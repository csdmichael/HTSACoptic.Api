
using System.Collections.Generic;
using HTSA.Models;
using System;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using System.IO;
using System.Threading;
using Microsoft.EntityFrameworkCore.Storage;
using Google.Apis.Util.Store;

//----------------------------------------------------------
//-- Author: Michael Yaacoub
//-- Email: csdmichael@gmail.com
//----------------------------------------------------------

namespace HTSA.Repositories
{
    public class CalendarRepository : ICalendarRepository
    {
        

        static string[] Scopes = { CalendarService.Scope.CalendarReadonly };
        static string ApplicationName = "Google Calendar API .NET Quickstart";

        ApplContext _context;

        public CalendarRepository(ApplContext context)
        {
            _context = context;
        }

        public List<GroupedCalEvents> GetGroupedEventsFromDB()
        {
            List<GroupedCalEvents> groupedCalEvents = new List<GroupedCalEvents>();

            List<CalEvent> calEvents = GetEventsFromDB();

            string currDay = "", prevDay = "";
            DateTime startDT;
            GroupedCalEvents grpEvent = null;

            foreach (CalEvent ev in calEvents)
            {
                startDT = System.Convert.ToDateTime(ev.StartDateTime);
                currDay = FormatDay(startDT);

                if (currDay != prevDay)
                {
                    grpEvent = new GroupedCalEvents(currDay);
                    grpEvent.Events.Add(ev);
                    groupedCalEvents.Add(
                        grpEvent
                    );
                }
                else
                {
                    if (grpEvent != null)
                    {
                        grpEvent.Events.Add(ev);
                    }
                }
                
                prevDay = currDay;
            }

            return groupedCalEvents;
        }

        public List<CalEvent> GetEventsFromDB()
        {
            List<CalEvent> calEvents = new List<CalEvent>();
            
            string sqlQuery = @"exec CLNDR.SP_Event_Get";

            return GetEventsFromDB_Query(sqlQuery);
        }

        public List<CalEvent> GetEventsFromDB_ByDay(string day)
        {
            List<CalEvent> calEvents = new List<CalEvent>();

            string sqlQuery = @"exec CLNDR.SP_Event_Get_ByDay @Day='" + day + "'";
            
            return GetEventsFromDB_Query(sqlQuery);
        }

        public List<CalEvent> GetEventsFromDB_Query(string sqlQuery)
        {
            List<CalEvent> calEvents = new List<CalEvent>();

            try
            {
                RelationalDataReader dr = _context.Database.ExecuteSqlQuery(sqlQuery);

                if (dr.DbDataReader.HasRows)
                {
                    while (dr.DbDataReader.Read())
                    {
                        CalEvent currEvent = new CalEvent();
                        currEvent.Description = dr.DbDataReader["Description"].ToString();
                        currEvent.Summary = dr.DbDataReader["Summary"].ToString();
                        currEvent.Status = dr.DbDataReader["Status"].ToString();
                        currEvent.StartDate = dr.DbDataReader["StartDate"].ToString();

                        string startDT = dr.DbDataReader["StartDateTime"].ToString();
                        currEvent.StartDateTime = System.Convert.ToDateTime(startDT);
                        currEvent.EndDate = dr.DbDataReader["EndDate"].ToString();

                        string endDT = dr.DbDataReader["EndDateTime"].ToString();
                        currEvent.EndDateTime = System.Convert.ToDateTime(endDT);
                        currEvent.Id = dr.DbDataReader["Id"].ToString();
                        currEvent.LinkURL = dr.DbDataReader["LinkURL"].ToString();

                        string lastUpdatedDT = dr.DbDataReader["LastUpdated"].ToString();

                        currEvent.LastUpdated = System.Convert.ToDateTime(lastUpdatedDT);

                        currEvent.DetailsHtml = dr.DbDataReader["DetailsHtml"].ToString();
                        currEvent.PosterId = dr.DbDataReader["PosterId"].ToString();

                        if (startDT.Length > 0)
                            currEvent.StartTime = startDT.Split(' ')[1] + " " + startDT.Split(' ')[2];

                        currEvent.StartTime = currEvent.StartTime.Replace(":00 AM", " AM").Replace(":00 PM", " PM");

                        if (endDT.Length > 0)
                            currEvent.EndTime = endDT.Split(' ')[1] + " " + endDT.Split(' ')[2];

                        currEvent.EndTime = currEvent.EndTime.Replace(":00 AM", " AM").Replace(":00 PM", " PM");

                        calEvents.Add(currEvent);
                    }
                }

                dr.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return calEvents;
        }

        public List<CalEvent> GetEventsFromDB_Today()
        {
            string strToday = FormatDay(DateTime.Now);
            return GetEventsFromDB_ByDay(strToday);
        }

        public List<CalEvent> GetEventsFromDB_Tomorrow()
        {
            string strTomorrow = FormatDay(DateTime.Now.AddDays(1));
            return GetEventsFromDB_ByDay(strTomorrow);
        }

        public List<CalEvent> GetEvents(string calendarName)
        {
            List<CalEvent> calEvents = new List<CalEvent>();
            IList<Event> events;

            try
            {
                UserCredential credential;

                using (var stream =
                    new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
                {
                    // The file token.json stores the user's access and refresh tokens, and is created
                    // automatically when the authorization flow completes for the first time.
                    string credPath = "token.json";
                    StoredResponse myStoredResponse = new StoredResponse();
                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.Load(stream).Secrets,
                        Scopes,
                        "user",
                        CancellationToken.None,
                        new SavedDataStore(myStoredResponse)).Result;
                    //new FileDataStore(credPath, true)).Result;

                    Console.WriteLine("Credential file saved to: " + credPath);
                }

                // Create Google Calendar API service.
                var service = new CalendarService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName,
                });

                // Define parameters of request.
                EventsResource.ListRequest request = service.Events.List(calendarName);
                request.TimeMin = DateTime.Now;
                request.ShowDeleted = false;
                request.SingleEvents = true;
                request.MaxResults = 10000;
                request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

                // List events.
                events = request.Execute().Items;

                foreach(Event ev in events)
                {
                    CalEvent currEvent = new CalEvent();

                    currEvent.Id = ev.Id;
                    currEvent.Status = ev.Status;
                    currEvent.Summary = ev.Summary;
                    currEvent.Description = ev.Description;
                    currEvent.StartDate = ev.Start.Date;
                    currEvent.StartDateTime = ev.Start.DateTime.Value.AddHours(3);
                    currEvent.LinkURL = ev.HtmlLink;
                    currEvent.LastUpdated = ev.Updated;

                    currEvent.EndDate = ev.End.Date;
                    currEvent.EndDateTime = ev.End.DateTime.Value.AddHours(3);

                    calEvents.Add(currEvent);
                }

                //Console.WriteLine("Upcoming events:");
                //if (events.Items != null && events.Items.Count > 0)
                //{
                //    foreach (var eventItem in events.Items)
                //    {
                //        string when = eventItem.Start.DateTime.ToString();
                //        if (String.IsNullOrEmpty(when))
                //        {
                //            when = eventItem.Start.Date;
                //        }
                //        Console.WriteLine("{0} ({1})", eventItem.Summary, when);
                //    }
                //}
                //else
                //{
                //    Console.WriteLine("No upcoming events found.");
                //}
                //Console.Read();
            }
            catch(Exception ex)
            { }
            return calEvents;

        }

        private string FormatDate(DateTime? dt1)
        {
            string apm = "AM";

            if (dt1 == null) return "";
            DateTime dt = (DateTime) dt1;
            string year = dt.Year.ToString();
            string month = dt.Month.ToString();
            if (month.Length == 1)
                month = "0" + month;

            string day = dt.Day.ToString();

            if (day.Length == 1)
                day = "0" + day;

            string hour = dt.Hour.ToString();

            

            if (dt.Hour > 12)
            {
                hour = (dt.Hour - 12).ToString();
                apm = "PM";
            }

            if (dt.Hour == 0)
            {
                hour = "12";
                apm = "AM";
            }

            if (dt.Hour == 12)
            {
                hour = "12";
                apm = "PM";
            }

            if (hour.Length == 1)
                hour = "0" + hour;

            

            string min = dt.Minute.ToString();

            if (min.Length == 1)
                min = "0" + min;

            string sec = dt.Second.ToString();

            if (sec.Length == 1)
                sec = "0" + sec;

            /*
            string mSec = dt.Millisecond.ToString();

            if (mSec.Length == 0)
                mSec = "000";

            if (mSec.Length == 1)
                mSec = "00" + mSec;

            if (mSec.Length == 2)
                mSec = "0" + mSec;
                */

            return
                String.Format("{0}-{1}-{2} {3}:{4}:{5} {6}",
                    year,
                    month,
                    day,
                    hour,
                    min,
                    sec,
                    apm);
        }

        private string FormatDay(DateTime? dt1)
        {
            string apm = "AM";

            if (dt1 == null) return "";
            DateTime dt = (DateTime)dt1;
            string year = dt.Year.ToString();
            string month = dt.Month.ToString();
            if (month.Length == 1)
                month = "0" + month;

            string day = dt.Day.ToString();

            if (day.Length == 1)
                day = "0" + day;

           

            return
                String.Format("{0}-{1}-{2}",
                    year,
                    month,
                    day);
        }

        public bool DataRefreshEvents(List<CalEvent> lstCalEvents)
        {
            try
            {
                string sqlQuery;

                //Clear STG_EvENTS Table
                //---------------------------
                sqlQuery = @"exec CLNDR.SP_Event_STG_ClearAll";
                //int paramCnt = 0;

                RelationalDataReader dr = _context.Database.ExecuteSqlQuery(sqlQuery);
                dr.Dispose();
                
                foreach(CalEvent currEvent in lstCalEvents)
                {
                    try
                    {
                        sqlQuery =
                            @"exec CLNDR.SP_Event_STG_Add "
                         + string.Format("@Id = '{0}',@Description = N'{1}', @LinkURL = '{2}',@StartDateTime = '{3}', @StartDate = '{4}', @EndDateTime = '{5}', @EndDate = '{6}', @Summary = N'{7}', @Status = '{8}', @LastUpdated = '{9}'",
                                currEvent.Id,
                                (currEvent.Description == null)? "": currEvent.Description.Replace("'", "''"),
                                currEvent.LinkURL,
                                FormatDate(currEvent.StartDateTime),
                                currEvent.StartDate,
                                FormatDate(currEvent.EndDateTime),
                                currEvent.EndDate,
                                (currEvent.Summary == null)? "": currEvent.Summary.Replace("'", "''"),
                                currEvent.Status,
                                FormatDate(currEvent.LastUpdated)
                            );
                        //int paramCnt = 0;

                        dr = _context.Database.ExecuteSqlQuery(sqlQuery);
                        dr.Dispose();
                    }
                    catch(Exception ex1)
                    {
                        Console.WriteLine("Error in event {" + currEvent.Description + "}. Error Msg: " + ex1.Message);
                    }
                }

                sqlQuery = @"exec CLNDR.SP_Event_LoadFromSTG";
                //int paramCnt = 0;

                dr = _context.Database.ExecuteSqlQuery(sqlQuery);
                dr.Dispose();

            }
            catch(Exception ex)
            {
                return false;
            }
            return true;
        }



    }
}
