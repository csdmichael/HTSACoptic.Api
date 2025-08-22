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
    public class LoginsInfoRepository : ILoginsInfoRepository
    {
        ILogger _logger;
        ApplContext _context;
        IAuthRepository _tokenInfoRepository;

        public LoginsInfoRepository(ApplContext context, ILogger<LoginsInfoRepository> logger, IAuthRepository tokenInfoRepository)
        {
            _logger = logger;
            _context = context;
            _tokenInfoRepository = tokenInfoRepository;
        }

        public LoginsByDayResponse GetLoginsByDay(string baseURL, string token)
        {
            //----------------------------------------------------------
            //-- Michael Yaacoub - 12/21/2016
            //-- csdmichael@gmail.com
            //----------------------------------------------------------
            LoginsByDayResponse lgnsr = new LoginsByDayResponse();
            var lstLoginsInfo = new List<LoginsByDayInfo>();

            try
            {

                string sqlQuery;

                sqlQuery = @"exec [AUTH].[SP_LoginsByDay]";

                var rdr = _context.Database.ExecuteSqlQuery(sqlQuery);

                while (rdr.DbDataReader.Read())
                {
                    LoginsByDayInfo lgnInfo = new LoginsByDayInfo();

                    lgnInfo.Year = System.Convert.ToInt32(rdr.DbDataReader["LoginYear"]);
                    lgnInfo.Month = System.Convert.ToInt32(rdr.DbDataReader["LoginMonth"]);
                    lgnInfo.Day = System.Convert.ToInt32(rdr.DbDataReader["LoginDay"]);
                    lgnInfo.LoginsCount = System.Convert.ToInt32(rdr.DbDataReader["LoginsCount"]);
                    lgnInfo.SuccessLoginsCount = System.Convert.ToInt32(rdr.DbDataReader["SuccessLoginsCount"]);
                    lgnInfo.DateValue = lgnInfo.Year.ToString() + "-" + lgnInfo.Month.ToString() + "-" + lgnInfo.Day.ToString();

                    lstLoginsInfo.Add(lgnInfo);
                }
                rdr.DbDataReader.Dispose();

            }
            catch (Exception ex)
            {
                _context.Database.ExecuteSqlQuery(@"
				exec [LOGS].[SP_LogApplicationMsg] @Type='ERROR', @Message='" + ex.Message + "', @Module='LoginsInfoRepository.GetLoginsByDay'");
            }
            lgnsr.LoginsByDayList = lstLoginsInfo;
            lgnsr.LoginsByDayListCount = lstLoginsInfo.Count;
            return lgnsr;
        }


        public LoginsByModuleResponse GetLoginsByModule(string baseURL, string token)
        {
            //----------------------------------------------------------
            //-- Michael Yaacoub - 12/21/2016
            //-- csdmichael@gmail.com
            //----------------------------------------------------------
            LoginsByModuleResponse lgnsr = new LoginsByModuleResponse();
            var lstLoginsInfo = new List<LoginsByModuleInfo>();

            try
            {
                string sqlQuery;

                sqlQuery = @"exec [AUTH].[SP_LoginsByModule]";

                var rdr = _context.Database.ExecuteSqlQuery(sqlQuery);

                while (rdr.DbDataReader.Read())
                {
                    LoginsByModuleInfo lgnInfo = new LoginsByModuleInfo();

                    lgnInfo.Module = rdr.DbDataReader["Module"].ToString();
                    lgnInfo.LoginsCount = System.Convert.ToInt32(rdr.DbDataReader["LoginsCount"]);

                    lstLoginsInfo.Add(lgnInfo);
                }
                rdr.DbDataReader.Dispose();

            }
            catch (Exception ex)
            {
                _context.Database.ExecuteSqlQuery(@"
				exec [LOGS].[SP_LogApplicationMsg] @Type='ERROR', @Message='" + ex.Message + "', @Module='LoginsInfoRepository.GetLoginsByModule'");
            }
            lgnsr.LoginsByModuleList = lstLoginsInfo;
            lgnsr.LoginsByModuleListCount = lstLoginsInfo.Count;
            return lgnsr;
        }

        public LoginsByUserIDResponse GetLoginsByUserID(string baseURL, string token)
        {
            //----------------------------------------------------------
            //-- Michael Yaacoub - 12/21/2016
            //-- csdmichael@gmail.com
            //----------------------------------------------------------
            LoginsByUserIDResponse lgnsr = new LoginsByUserIDResponse();
            var lstLoginsInfo = new List<LoginsByUserIDInfo>();

            try
            {

                string sqlQuery;

                sqlQuery = @"exec [AUTH].[SP_LoginsByUserID]";

                var rdr = _context.Database.ExecuteSqlQuery(sqlQuery);

                while (rdr.DbDataReader.Read())
                {
                    LoginsByUserIDInfo lgnInfo = new LoginsByUserIDInfo();

                    lgnInfo.UserID = rdr.DbDataReader["UserID"].ToString();
                    lgnInfo.LoginsCount = System.Convert.ToInt32(rdr.DbDataReader["LoginsCount"]);

                    lstLoginsInfo.Add(lgnInfo);
                }
                rdr.DbDataReader.Dispose();

            }
            catch (Exception ex)
            {
                _context.Database.ExecuteSqlQuery(@"
				exec [LOGS].[SP_LogApplicationMsg] @Type='ERROR', @Message='" + ex.Message + "', @Module='LoginsInfoRepository.GetLoginsByUserID'");
            }
            lgnsr.LoginsByUserIDList = lstLoginsInfo;
            lgnsr.LoginsByUserIDListCount = lstLoginsInfo.Count;
            return lgnsr;
        }

        public LoginDetailsResponse GetLoginDetails(string date, string module, string userID, string baseURL, string token)
        {
            //----------------------------------------------------------
            //-- Michael Yaacoub - 12/21/2016
            //-- csdmichael@gmail.com
            //----------------------------------------------------------
            LoginDetailsResponse lgnsr = new LoginDetailsResponse();
            var lstLoginsInfo = new List<LoginDetailsInfo>();

            try
            {
                if (date == null) date = "";
                if (module == null) module = "";
                if (userID == null) userID = "";
                string sqlQuery;

                sqlQuery = @"exec [AUTH].[SP_LoginsDetails]";

                int paramCnt = 0;

                if (module.Trim() != "")
                {
                    sqlQuery += " @Module = '" + module + "'";
                    paramCnt++;
                }

                if (userID.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @UserID = '" + userID + "'";
                    paramCnt++;
                }

                if (date.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @StartDate='" + date + "'";
                    sqlQuery += ", ";
                    sqlQuery += "@EndDate='" + date + "'";
                    paramCnt++;
                }

                var rdr = _context.Database.ExecuteSqlQuery(sqlQuery);

                while (rdr.DbDataReader.Read())
                {
                    LoginDetailsInfo lgnInfo = new LoginDetailsInfo();

                    lgnInfo.LoginAttemptID = System.Convert.ToInt32(rdr.DbDataReader["LoginAttemptID"].ToString());
                    lgnInfo.UserID = rdr.DbDataReader["UserID"].ToString();
                    lgnInfo.Token = rdr.DbDataReader["Token"].ToString();
                    lgnInfo.IsLoginSuccess = System.Convert.ToBoolean(rdr.DbDataReader["IsLoginSuccess"]);
                    lgnInfo.Module = rdr.DbDataReader["Module"].ToString();
                    lgnInfo.Params = rdr.DbDataReader["Params"].ToString();
                    lgnInfo.LoginDateTime = rdr.DbDataReader["LoginDateTime"].ToString();
                    lgnInfo.ParamsList = new List<ParamValue>();
                    if (!String.IsNullOrEmpty(lgnInfo.Params))
                    {
                        string[] paramsArr = lgnInfo.Params.Split(',');
                        foreach (string str in paramsArr)
                        {
                            string paramName = str.Split('=')[0].Trim().Replace("@", "");
                            string paramVal = "";
                            if (str.Split('=')[1] != null)
                                paramVal = str.Split('=')[1].Trim();
                            else
                                paramVal = "";

                            ParamValue pv = new ParamValue();
                            pv.ParamName = paramName;
                            pv.ParamVal = paramVal;

                            lgnInfo.ParamsList.Add(pv);
                        }
                        lgnInfo.ParamsListCount = lgnInfo.ParamsList.Count;
                    }
                    else
                    {
                        lgnInfo.ParamsListCount = 0;
                    }

                    lstLoginsInfo.Add(lgnInfo);
                }
                rdr.DbDataReader.Dispose();
            }

            catch (Exception ex)
            {
                _context.Database.ExecuteSqlQuery(@"
				exec [LOGS].[SP_LogApplicationMsg] @Type='ERROR', @Message='" + ex.Message + "', @Module='LoginsInfoRepository.GetLoginDetails'");
            }
            lgnsr.LoginDetailsList = lstLoginsInfo;
            lgnsr.LoginDetailsListCount = lstLoginsInfo.Count;
            return lgnsr;
        }
    }
}
