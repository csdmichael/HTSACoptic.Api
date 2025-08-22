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
    public class KidRepository : IKidRepository
    {
        ILogger _logger;
        ApplContext _context;
        ITokenInfoRepository _tokenInfoRepository;

        public KidRepository(ApplContext context, ILogger<KidRepository> logger, ITokenInfoRepository tokenInfoRepository)
        {
            _logger = logger;
            _context = context;
            _tokenInfoRepository = tokenInfoRepository;
        }

        public KidsResponse GetKidsByClass(string baseURL, string token, string classID)
        {
            //----------------------------------------------------------
            //-- Michael Yaacoub - 10/06/2017
            //-- csdmichael@gmail.com
            //----------------------------------------------------------
            KidsResponse KidsResp = new KidsResponse();
            var lstKids = new List<Kid>();

            try
            {
                AuthResponse auth = _tokenInfoRepository.VerifyToken(token, "GetKidsByClass", "");
                KidsResp.authResponse = auth;

                if (auth.success)
                {
                    string sqlQuery;

                    sqlQuery = @"exec SDS.[SP_KidsByClass_Get]";
                    int paramCnt = 0;

                    if (classID.Trim() != "")
                    {
                        sqlQuery += " @ClassID = '" + classID + "'";
                        paramCnt++;
                    }
                    var rdr = _context.Database.ExecuteSqlQuery(sqlQuery);

                    while (rdr.DbDataReader.Read())
                    {
                        Kid kd = new Kid();

                        kd.PersonID = rdr.DbDataReader["KidID"].ToString();
                        kd.PersonName = rdr.DbDataReader["KidName"].ToString();
                        kd.CellPhone = rdr.DbDataReader["CellPhone"].ToString();
                        kd.Email = rdr.DbDataReader["Email"].ToString();
                        kd.DOB = rdr.DbDataReader["DOB"].ToString();
                        kd.FatherID = rdr.DbDataReader["FatherID"].ToString();
                        kd.FatherName = rdr.DbDataReader["FatherName"].ToString();
                        kd.MotherID = rdr.DbDataReader["MotherID"].ToString();
                        kd.MotherName = rdr.DbDataReader["MotherName"].ToString();
                        kd.YearsOld = System.Convert.ToInt32(rdr.DbDataReader["YearsOld"]);

                        kd.AttendPerc = System.Convert.ToInt32(rdr.DbDataReader["AttendPerc"]);
                        kd.AttendCount = System.Convert.ToInt32(rdr.DbDataReader["AttendCount"]);
                        kd.LessonsCount = System.Convert.ToInt32(rdr.DbDataReader["LessonsCount"]);

                        kd.PictureName = rdr.DbDataReader["PictureName"].ToString();

                        lstKids.Add(kd);
                    }
                    rdr.DbDataReader.Dispose();
                }
            }
            catch (Exception ex)
            {
                _context.Database.ExecuteSqlQuery(@"
				exec [LOGS].[SP_LogApplicationMsg] @Type='ERROR', @Message='" + ex.Message + "', @Module='KidRepository.GetKidsByClass'");
            }
            KidsResp.KidsList = lstKids;
            KidsResp.KidsListCount = lstKids.Count;
            return KidsResp;
        }


    }
}
