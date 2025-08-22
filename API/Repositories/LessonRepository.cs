using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using HTSA.Models;
using System;
//----------------------------------------------------------
//-- Author: Michael Yaacoub
//-- Email: csdmichael@gmail.com
//----------------------------------------------------------

namespace HTSA.Repositories
{
    public class LessonRepository : ILessonRepository
    {
        ILogger _logger;
        ApplContext _context;
        ITokenInfoRepository _tokenInfoRepository;

        public LessonRepository(ApplContext context, ILogger<LessonRepository> logger, ITokenInfoRepository tokenInfoRepository)
        {
            _logger = logger;
            _context = context;
            _tokenInfoRepository = tokenInfoRepository;
        }

        public LessonsResponse GetLessonsByClass(string baseURL, string token, string classID, string IsThisWeek)
        {
            //----------------------------------------------------------
            //-- Michael Yaacoub - 10/06/2017
            //-- csdmichael@gmail.com
            //----------------------------------------------------------
            LessonsResponse lessonsResp = new LessonsResponse();
            var lstLessons = new List<Lesson>();

            try
            {
                AuthResponse auth = _tokenInfoRepository.VerifyToken(token, "GetLessonsByClass", "");
                lessonsResp.authResponse = auth;

                if (auth.success)
                {
                    string sqlQuery;

                    sqlQuery = @"exec SDS.[SP_LessonsByClass_Get]";
					int paramCnt = 0;

                    if (classID.Trim() != "")
                    {
                        sqlQuery += " @ClassID = '" + classID + "'";
                        paramCnt++;
                    }

                    if (IsThisWeek != null && IsThisWeek.Trim() != "")
                    {
                        if (paramCnt > 0)
                            sqlQuery += ", ";
                        sqlQuery += " @IsThisWeek = '" + IsThisWeek + "'";
                        paramCnt++;
                    }

                    var rdr = _context.Database.ExecuteSqlQuery(sqlQuery);

                    while (rdr.DbDataReader.Read())
                    {
                        Lesson les = new Lesson();

                        les.ClassID = System.Convert.ToInt32(rdr.DbDataReader["ClassID"]);
                        les.ChurchID = rdr.DbDataReader["ChurchID"].ToString();
                        les.ClassName = rdr.DbDataReader["ClassName"].ToString();
                        les.StartDate = rdr.DbDataReader["StartDate"].ToString();
                        les.FinishDate = rdr.DbDataReader["FinishDate"].ToString();
                        les.Grade = rdr.DbDataReader["Grade"].ToString();
                        les.Sex = rdr.DbDataReader["Sex"].ToString();
						les.LessonID = System.Convert.ToInt32(rdr.DbDataReader["LessonID"]);
						les.DateKey = rdr.DbDataReader["DateKey"].ToString();
                        les.Date = rdr.DbDataReader["Date"].ToString();
                        les.LessonName = rdr.DbDataReader["LessonName"].ToString();
                        les.LessonDescr = rdr.DbDataReader["LessonDescr"].ToString();
						
						
                        les.AttendCount = System.Convert.ToInt32(rdr.DbDataReader["AttendCount"]);
                        les.TotalCount = System.Convert.ToInt32(rdr.DbDataReader["TotalCount"]);
                        les.AttendPerc = System.Convert.ToInt32(rdr.DbDataReader["AttendPerc"]);

                        lstLessons.Add(les);
                    }
                    rdr.DbDataReader.Dispose();
                }
            }
            catch (Exception ex)
            {
                _context.Database.ExecuteSqlQuery(@"
				exec [LOGS].[SP_LogApplicationMsg] @Type='ERROR', @Message='" + ex.Message + "', @Module='LessonRepository.GetLessonsByClass'");
            }
            lessonsResp.LessonsList = lstLessons;
            lessonsResp.LessonsListCount = lstLessons.Count;
            return lessonsResp;
        }

        public LessonsResponse DeleteLesson(string baseURL, string token, string lessonID)
        {
            //----------------------------------------------------------
            //-- Michael Yaacoub - 10/06/2017
            //-- csdmichael@gmail.com
            //----------------------------------------------------------
            LessonsResponse lessonsResp = new LessonsResponse();
            var lstLessons = new List<Lesson>();

            try
            {
                AuthResponse auth = _tokenInfoRepository.VerifyToken(token, "DeleteLesson", "");
                lessonsResp.authResponse = auth;

                if (auth.success)
                {
                    string sqlQuery;

                    sqlQuery = @"exec SDS.[SP_ClassLesson_Delete]";
                    int paramCnt = 0;

                    if (lessonID.Trim() != "")
                    {
                        sqlQuery += " @LessonID = '" + lessonID + "'";
                        paramCnt++;
                    }

                    
                    var rdr = _context.Database.ExecuteSqlQuery(sqlQuery);

                    rdr.DbDataReader.Dispose();
                }
            }
            catch (Exception ex)
            {
                _context.Database.ExecuteSqlQuery(@"
				exec [LOGS].[SP_LogApplicationMsg] @Type='ERROR', @Message='" + ex.Message + "', @Module='LessonRepository.DeleteLesson'");
            }

            lessonsResp.LessonsList = lstLessons;
            lessonsResp.LessonsListCount = lstLessons.Count;
            return lessonsResp;
        }

        public LessonsResponse CreateNewLesson(string baseURL, string token, string lessonName, string lessonDescr, string classID, string nextSunday)
        {
            //----------------------------------------------------------
            //-- Michael Yaacoub - 10/07/2017
            //-- csdmichael@gmail.com
            //----------------------------------------------------------
            LessonsResponse lessonsResp = new LessonsResponse();
            var lstLessons = new List<Lesson>();

            try
            {
                AuthResponse auth = _tokenInfoRepository.VerifyToken(token, "CreateNewLesson", "");
                lessonsResp.authResponse = auth;
                

                if (auth.success)
                {
                    string sqlQuery;

                    sqlQuery = @"exec SDS.[SP_ClassLesson_New]";
                    int paramCnt = 0;

                    if (lessonName != null && lessonName.Trim() != "")
                    {
                        sqlQuery += " @LessonName = '" + lessonName + "'";
                        paramCnt++;
                    }

                    if (lessonDescr != null && lessonDescr.Trim() != "")
                    {
                        if (paramCnt > 0)
                            sqlQuery += ", ";
                        sqlQuery += " @LessonDescr = '" + lessonDescr + "'";
                        paramCnt++;
                    }

                    if (classID != null && classID.Trim() != "")
                    {
                        if (paramCnt > 0)
                            sqlQuery += ", ";
                        sqlQuery += " @ClassID = '" + classID + "'";
                        paramCnt++;
                    }

                    if (nextSunday != null && nextSunday.Trim() != "")
                    {
                        if (paramCnt > 0)
                            sqlQuery += ", ";
                        sqlQuery += " @NextSunday = '" + nextSunday + "'";
                        paramCnt++;
                    }

                    var rdr = _context.Database.ExecuteSqlQuery(sqlQuery);

                    while (rdr.DbDataReader.Read())
                    {
                        Lesson les = new Lesson();

                        les.LessonID = System.Convert.ToInt32(rdr.DbDataReader["LessonID"]);
                        les.DateKey = rdr.DbDataReader["DateKey"].ToString();
                        les.Date = rdr.DbDataReader["Date"].ToString();
                        les.LessonName = rdr.DbDataReader["LessonName"].ToString();
                        les.LessonDescr = rdr.DbDataReader["LessonDescr"].ToString();
                        les.ClassID = System.Convert.ToInt32(classID);

                        les.AttendCount = System.Convert.ToInt32(rdr.DbDataReader["AttendCount"]);
                        les.TotalCount = System.Convert.ToInt32(rdr.DbDataReader["TotalCount"]);
                        les.AttendPerc = System.Convert.ToInt32(rdr.DbDataReader["AttendPerc"]);

                        lstLessons.Add(les);
                    }
                    rdr.DbDataReader.Dispose();
                }
            }
            catch (Exception ex)
            {
                _context.Database.ExecuteSqlQuery(@"
				exec [LOGS].[SP_LogApplicationMsg] @Type='ERROR', @Message='" + ex.Message + "', @Module='LessonRepository.CreateNewLesson'");
            }
            lessonsResp.LessonsList = lstLessons;
            lessonsResp.LessonsListCount = lstLessons.Count;
            return lessonsResp;
        }


        public UpdateResponse UpdateLesson(string baseURL, string token, string lessonID, string lessonName, string lessonDescr, string nextSunday)
        {
            //----------------------------------------------------------
            //-- Michael Yaacoub - 10/07/2017
            //-- csdmichael@gmail.com
            //----------------------------------------------------------
            UpdateResponse ur = new UpdateResponse();
            

            try
            {
                AuthResponse auth = _tokenInfoRepository.VerifyToken(token, "UpdateLesson", "");
                ur.authResponse = auth;


                if (auth.success)
                {
                    string sqlQuery;

                    sqlQuery = @"exec SDS.[SP_ClassLesson_Update]";
                    int paramCnt = 0;

                    if (lessonID != null && lessonID.Trim() != "")
                    {
                        if (paramCnt > 0)
                            sqlQuery += ", ";
                        sqlQuery += " @LessonID = " + lessonID;
                        paramCnt++;
                    }

                    if (lessonName != null && lessonName.Trim() != "")
                    {
                        if (paramCnt > 0)
                            sqlQuery += ", ";
                        sqlQuery += " @LessonName = '" + lessonName + "'";
                        paramCnt++;
                    }

                    if (lessonDescr != null && lessonDescr.Trim() != "")
                    {
                        if (paramCnt > 0)
                            sqlQuery += ", ";
                        sqlQuery += " @LessonDescr = '" + lessonDescr + "'";
                        paramCnt++;
                    }

                    

                    if (nextSunday != null && nextSunday.Trim() != "")
                    {
                        if (paramCnt > 0)
                            sqlQuery += ", ";
                        sqlQuery += " @NextSunday = '" + nextSunday + "'";
                        paramCnt++;
                    }

                    var rdr = _context.Database.ExecuteSqlQuery(sqlQuery);
                    ur.success = true;
                    /*
                    while (rdr.DbDataReader.Read())
                    {
                        Lesson les = new Lesson();

                        les.LessonID = System.Convert.ToInt32(rdr.DbDataReader["LessonID"]);
                        les.DateKey = rdr.DbDataReader["DateKey"].ToString();
                        les.Date = rdr.DbDataReader["Date"].ToString();
                        les.LessonName = rdr.DbDataReader["LessonName"].ToString();
                        les.LessonDescr = rdr.DbDataReader["LessonDescr"].ToString();
                        les.ClassID = System.Convert.ToInt32(rdr.DbDataReader["ClassID"]);

                        les.AttendCount = System.Convert.ToInt32(rdr.DbDataReader["AttendCount"]);
                        les.TotalCount = System.Convert.ToInt32(rdr.DbDataReader["TotalCount"]);
                        les.AttendPerc = System.Convert.ToInt32(rdr.DbDataReader["AttendPerc"]);

                        lstLessons.Add(les);
                    }
                    */
                    rdr.DbDataReader.Dispose();
                }
            }
            catch (Exception ex)
            {
                ur.success = false;

                _context.Database.ExecuteSqlQuery(@"
				exec [LOGS].[SP_LogApplicationMsg] @Type='ERROR', @Message='" + ex.Message + "', @Module='LessonRepository.UpdateLesson'");
            }
            
            return ur;
        }

        public PersonResponse UpdateAttendance(string baseURL, string token, string lessonID, string personIDs, string isAttendList)
        {
            //----------------------------------------------------------
            //-- Michael Yaacoub - 10/07/2017
            //-- csdmichael@gmail.com
            //----------------------------------------------------------
            PersonResponse personResp = new PersonResponse();
            var lstPerson = new List<Person>();

            try
            {
                AuthResponse auth = _tokenInfoRepository.VerifyToken(token, "UpdateAttendance", "");
                personResp.authResponse = auth;


                if (auth.success)
                {
                    string sqlQuery;

                    sqlQuery = @"exec SDS.[SP_LessonAttend_Update]";
                    int paramCnt = 0;

                    if (lessonID != null && lessonID.Trim() != "")
                    {
                        sqlQuery += " @LessonID = '" + lessonID + "'";
                        paramCnt++;
                    }

                    if (personIDs != null && personIDs.Trim() != "")
                    {
                        if (paramCnt > 0)
                            sqlQuery += ", ";
                        sqlQuery += " @PersonIDs = '" + personIDs + "'";
                        paramCnt++;
                    }

                    if (isAttendList != null && isAttendList.Trim() != "")
                    {
                        if (paramCnt > 0)
                            sqlQuery += ", ";
                        sqlQuery += " @IsAttendList = '" + isAttendList + "'";
                        paramCnt++;
                    }

                    
                    var rdr = _context.Database.ExecuteSqlQuery(sqlQuery);

                    while (rdr.DbDataReader.Read())
                    {
                        Person pr = new Person();

                        pr.PersonID = rdr.DbDataReader["PersonID"].ToString();
                        pr.PersonName = rdr.DbDataReader["PersonName"].ToString();
                        pr.CellPhone = rdr.DbDataReader["CellPhone"].ToString();
                        pr.Email = rdr.DbDataReader["Email"].ToString();
                        pr.AttendPerc = rdr.DbDataReader["AttendCount"].ToString();

                        lstPerson.Add(pr);
                    }
                    rdr.DbDataReader.Dispose();
                }
            }
            catch (Exception ex)
            {
                _context.Database.ExecuteSqlQuery(@"
				exec [LOGS].[SP_LogApplicationMsg] @Type='ERROR', @Message='" + ex.Message + "', @Module='LessonRepository.UpdateAttendance'");
            }
            personResp.PersonList = lstPerson;
            personResp.PersonListCount = lstPerson.Count;
            return personResp;
        }

        public PersonResponse GetAttendance(string baseURL, string token, string lessonID)
        {
            //----------------------------------------------------------
            //-- Michael Yaacoub - 10/07/2017
            //-- csdmichael@gmail.com
            //----------------------------------------------------------
            PersonResponse personResp = new PersonResponse();
            var lstPerson = new List<Person>();

            try
            {
                AuthResponse auth = _tokenInfoRepository.VerifyToken(token, "GetAttendance", "");
                personResp.authResponse = auth;


                if (auth.success)
                {
                    string sqlQuery;

                    sqlQuery = @"exec SDS.[SP_AttendanceByLesson_Get]";
                    int paramCnt = 0;

                    if (lessonID != null && lessonID.Trim() != "")
                    {
                        sqlQuery += " @LessonID = '" + lessonID + "'";
                        paramCnt++;
                    }

                    
                    var rdr = _context.Database.ExecuteSqlQuery(sqlQuery);

                    while (rdr.DbDataReader.Read())
                    {
                        Person pr = new Person();

                        pr.PersonID = rdr.DbDataReader["PersonID"].ToString();
                        pr.PersonName = rdr.DbDataReader["PersonName"].ToString();
                        pr.CellPhone = rdr.DbDataReader["CellPhone"].ToString();
                        pr.Email = rdr.DbDataReader["Email"].ToString();
                        pr.AttendPerc = rdr.DbDataReader["AttendCount"].ToString();
                        lstPerson.Add(pr);
                    }
                    rdr.DbDataReader.Dispose();
                }
            }
            catch (Exception ex)
            {
                _context.Database.ExecuteSqlQuery(@"
				exec [LOGS].[SP_LogApplicationMsg] @Type='ERROR', @Message='" + ex.Message + "', @Module='LessonRepository.GetAttendance'");
            }
            personResp.PersonList = lstPerson;
            personResp.PersonListCount = lstPerson.Count;
            return personResp;
        }


    }
}
