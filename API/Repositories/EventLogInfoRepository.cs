using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using HTSA.Models;
using System;
using TokenAuth.Repositories;
//----------------------------------------------------------
//-- Author: Michael Yaacoub
//-- Email: csdmichael@gmail.com
//----------------------------------------------------------

namespace HTSA.Repositories
{
    public class EventLogInfoRepository : IEventLogInfoRepository
    {
        ILogger _logger;
        ApplContext _context;
        IAuthRepository _tokenInfoRepository;

        public EventLogInfoRepository(ApplContext context, ILogger<EventLogInfoRepository> logger, IAuthRepository tokenInfoRepository)
        {
            _logger = logger;
            _context = context;
            _tokenInfoRepository = tokenInfoRepository;
        }

        public EventLogResponse GetEventLogsInfo(string eventID, string module, string type, string messageKeyword, string startDate, string endDate, string baseURL, string token)
        {
            //----------------------------------------------------------
            //-- Michael Yaacoub - 12/21/2016
            //-- csdmichael@gmail.com
            //----------------------------------------------------------

            EventLogResponse elr = new EventLogResponse(); 
            var lstEventLogInfo = new List<EventLogInfo>();

            try
            {

                string sqlQuery;

                sqlQuery = @"exec [LOGS].[SP_GetApplicationMsg] @Type = '" + type + "', @MessageKeyword = '" + messageKeyword + "', @Module = '" + module + "'";
                if (eventID.Trim() != "")
                {
                    sqlQuery += ", @EventID = '" + eventID + "'";
                }

                if (startDate.Trim() != "")
                {
                    sqlQuery += ", @StartDate='" + startDate + "'";
                }

                if (endDate.Trim() != "")
                {
                    sqlQuery += ", @EndDate='" + endDate + "'";
                }


                var rdr = _context.Database.ExecuteSqlQuery(sqlQuery);

                while (rdr.DbDataReader.Read())
                {
                    EventLogInfo elInfo = new EventLogInfo();
                    elInfo.EventID = rdr.DbDataReader["EventID"].ToString();
                    elInfo.CountOrCode = rdr.DbDataReader["CountOrCode"].ToString();
                    elInfo.EventTime = rdr.DbDataReader["EventTime"].ToString();
                    elInfo.Message = rdr.DbDataReader["Message"].ToString();
                    elInfo.Module = rdr.DbDataReader["Module"].ToString();
                    elInfo.Type = rdr.DbDataReader["Type"].ToString();

                    lstEventLogInfo.Add(elInfo);
                }
                rdr.DbDataReader.Dispose();

            }
            catch (Exception ex)
            {
                _context.Database.ExecuteSqlQuery(@"
				exec [LOGS].[SP_LogApplicationMsg] @Type='ERROR', @Message='" + ex.Message + "', @Module='EventLogInfoRepository.GetEventLogsInfo'");
            }
            elr.EventLogsList = lstEventLogInfo;
            elr.EventLogsListCount = lstEventLogInfo.Count;
            return elr;
        }

        public EventLogResponse GetRowCountsInfo(string eventID, string messageKeyword, string startDate, string endDate, string baseURL, string token)
        {
            //----------------------------------------------------------
            //-- Michael Yaacoub - 12/21/2016
            //-- csdmichael@gmail.com
            //----------------------------------------------------------

            EventLogResponse elr = null;

            try
            {              
                elr = GetEventLogsInfo(eventID, "ROWCOUNT", "INFO", messageKeyword, startDate, endDate, baseURL, token);
            }
            catch (Exception ex)
            {
                _context.Database.ExecuteSqlQuery(@"
				exec [LOGS].[SP_LogApplicationMsg] @Type='ERROR', @Message='" + ex.Message + "', @Module='EventLogInfoRepository.GetEventLogsInfo'");
            }

            return elr;
        }
    }
}
