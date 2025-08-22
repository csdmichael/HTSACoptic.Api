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
    public class SSClassRepository : ISSClassRepository
    {
        ILogger _logger;
        ApplContext _context;
        ITokenInfoRepository _tokenInfoRepository;

        public SSClassRepository(ApplContext context, ILogger<SSClassRepository> logger, ITokenInfoRepository tokenInfoRepository)
        {
            _logger = logger;
            _context = context;
            _tokenInfoRepository = tokenInfoRepository;
        }

        public UpdateResponse AddClass(string baseURL, SSClass ssClass)
        {
            //----------------------------------------------------------
            //-- Michael Yaacoub - 10/06/2017
            //-- csdmichael@gmail.com
            //----------------------------------------------------------
            UpdateResponse updateResp = new UpdateResponse();

            try
            {
                AuthResponse auth = _tokenInfoRepository.VerifyToken(ssClass.Token, "AddClass", "");
                updateResp.authResponse = auth;

                if (auth.success)
                {
                    string sqlQuery;

                    sqlQuery = @"exec SDS.[SP_Class_Add]";
                    int paramCnt = 0;

                    if (ssClass.ChurchID != null && ssClass.ChurchID.Trim() != "")
                    {
                        sqlQuery += " @ChurchID = '" + ssClass.ChurchID + "'";
                        paramCnt++;
                    }

                    if (ssClass.ClassName != null && ssClass.ClassName.Trim() != "")
                    {
                        if (paramCnt > 0)
                            sqlQuery += ", ";
                        sqlQuery += " @ClassName = '" + ssClass.ClassName + "'";
                        paramCnt++;
                    }

                    if (ssClass.StartDate != null && ssClass.StartDate.Trim() != "")
                    {
                        if (paramCnt > 0)
                            sqlQuery += ", ";
                        sqlQuery += " @StartDate = '" + ssClass.StartDate + "'";
                        paramCnt++;
                    }

                    if (ssClass.FinishDate != null && ssClass.FinishDate.Trim() != "")
                    {
                        if (paramCnt > 0)
                            sqlQuery += ", ";
                        sqlQuery += " @FinishDate = '" + ssClass.FinishDate + "'";
                        paramCnt++;
                    }

                    if (ssClass.Grade != null && ssClass.Grade.Trim() != "")
                    {
                        if (paramCnt > 0)
                            sqlQuery += ", ";
                        sqlQuery += " @Grade = '" + ssClass.Grade + "'";
                        paramCnt++;
                    }

                    if (ssClass.Sex != null && ssClass.Sex.Trim() != "")
                    {
                        if (paramCnt > 0)
                            sqlQuery += ", ";
                        sqlQuery += " @Sex = '" + ssClass.Sex + "'";
                        paramCnt++;
                    }

                    if (ssClass.SaintID != null && ssClass.SaintID.Trim() != "")
                    {
                        if (paramCnt > 0)
                            sqlQuery += ", ";
                        sqlQuery += " @SaintID = '" + ssClass.SaintID + "'";
                        paramCnt++;
                    }

                    var rdr = _context.Database.ExecuteSqlQuery(sqlQuery);

                    updateResp.success = true;
                    rdr.DbDataReader.Dispose();
                }
            }
            catch (Exception ex)
            {
                updateResp.success = false;
                _context.Database.ExecuteSqlQuery(@"
				exec [LOGS].[SP_LogApplicationMsg] @Type='ERROR', @Message='" + ex.Message + "', @Module='SSClassRepository.AddClass'");
            }
            return updateResp;
        
        }

        public UpdateResponse UpdateClass(string baseURL, SSClass ssClass)
        {
            //----------------------------------------------------------
            //-- Michael Yaacoub - 10/06/2017
            //-- csdmichael@gmail.com
            //----------------------------------------------------------
            UpdateResponse updateResp = new UpdateResponse();

            try
            {
                AuthResponse auth = _tokenInfoRepository.VerifyToken(ssClass.Token, "UpdateClass", "");
                updateResp.authResponse = auth;

                if (auth.success)
                {
                    string sqlQuery;

                    sqlQuery = @"exec SDS.[SP_Class_Update]";
                    int paramCnt = 0;

                    if (ssClass.ChurchID != null && ssClass.ChurchID.Trim() != "")
                    {
                        sqlQuery += " @ClassID = '" + ssClass.ClassID + "'";
                        paramCnt++;
                    }

                    if (ssClass.ChurchID != null && ssClass.ChurchID.Trim() != "")
                    {
                        sqlQuery += " @ChurchID = '" + ssClass.ChurchID + "'";
                        paramCnt++;
                    }

                    if (ssClass.ClassName != null && ssClass.ClassName.Trim() != "")
                    {
                        if (paramCnt > 0)
                            sqlQuery += ", ";
                        sqlQuery += " @ClassName = '" + ssClass.ClassName + "'";
                        paramCnt++;
                    }

                    if (ssClass.StartDate != null && ssClass.StartDate.Trim() != "")
                    {
                        if (paramCnt > 0)
                            sqlQuery += ", ";
                        sqlQuery += " @StartDate = '" + ssClass.StartDate + "'";
                        paramCnt++;
                    }

                    if (ssClass.FinishDate != null && ssClass.FinishDate.Trim() != "")
                    {
                        if (paramCnt > 0)
                            sqlQuery += ", ";
                        sqlQuery += " @FinishDate = '" + ssClass.FinishDate + "'";
                        paramCnt++;
                    }

                    if (ssClass.Grade != null && ssClass.Grade.Trim() != "")
                    {
                        if (paramCnt > 0)
                            sqlQuery += ", ";
                        sqlQuery += " @Grade = '" + ssClass.Grade + "'";
                        paramCnt++;
                    }

                    if (ssClass.Sex != null && ssClass.Sex.Trim() != "")
                    {
                        if (paramCnt > 0)
                            sqlQuery += ", ";
                        sqlQuery += " @Sex = '" + ssClass.Sex + "'";
                        paramCnt++;
                    }

                    if (ssClass.SaintID != null && ssClass.SaintID.Trim() != "")
                    {
                        if (paramCnt > 0)
                            sqlQuery += ", ";
                        sqlQuery += " @SaintID = '" + ssClass.SaintID + "'";
                        paramCnt++;
                    }

                    var rdr = _context.Database.ExecuteSqlQuery(sqlQuery);

                    
                    rdr.DbDataReader.Dispose();

                    updateResp.success = true;

                }
            }
            catch (Exception ex)
            {
                updateResp.success = false;
                _context.Database.ExecuteSqlQuery(@"
				exec [LOGS].[SP_LogApplicationMsg] @Type='ERROR', @Message='" + ex.Message + "', @Module='SSClassRepository.UpdateClass'");
            }
            
            return updateResp;

        }

        public UpdateResponse DeleteClass(string baseURL, SSClass ssClass)
        {
            //----------------------------------------------------------
            //-- Michael Yaacoub - 10/06/2017
            //-- csdmichael@gmail.com
            //----------------------------------------------------------
            UpdateResponse updateResp = new UpdateResponse();

            try
            {
                AuthResponse auth = _tokenInfoRepository.VerifyToken(ssClass.Token, "DeleteClass", "");
                updateResp.authResponse = auth;

                if (auth.success)
                {
                    string sqlQuery;

                    sqlQuery = @"exec SDS.[SP_Class_Delete]";
                    int paramCnt = 0;

                    sqlQuery += " @ClassID = '" + ssClass.ClassID + "'";
                    paramCnt++;


                    var rdr = _context.Database.ExecuteSqlQuery(sqlQuery);
                    rdr.DbDataReader.Dispose();
                    updateResp.success = true;
                }
            }
            catch (Exception ex)
            {
                updateResp.success = false;
                _context.Database.ExecuteSqlQuery(@"
				exec [LOGS].[SP_LogApplicationMsg] @Type='ERROR', @Message='" + ex.Message + "', @Module='SSClassRepository.DeleteClass'");
            }
           
            return updateResp;

        }

        public UpdateResponse AddMember(string baseURL, ClassPerson cp)
        {
            //----------------------------------------------------------
            //-- Michael Yaacoub - 10/06/2017
            //-- csdmichael@gmail.com
            //----------------------------------------------------------
            UpdateResponse updateResp = new UpdateResponse();

            try
            {
                AuthResponse auth = _tokenInfoRepository.VerifyToken(cp.Token, "AddMember", "");
                updateResp.authResponse = auth;

                if (auth.success)
                {
                    string sqlQuery;

                    sqlQuery = @"exec SDS.[SP_Class_AddMember]";
                    int paramCnt = 0;

                    if (cp.ClassID != null && cp.ClassID.Trim() != "")
                    {
                        if (paramCnt > 0)
                            sqlQuery += ", ";
                        sqlQuery += " @ClassID = '" + cp.ClassID + "'";
                        paramCnt++;
                    }

                    if (cp.PersonID != null && cp.PersonID.Trim() != "")
                    {
                        if (paramCnt > 0)
                            sqlQuery += ", ";
                        sqlQuery += " @PersonID = '" + cp.PersonID + "'";
                        paramCnt++;
                    }

                    if (cp.ClassRole != null && cp.ClassRole.Trim() != "")
                    {
                        if (paramCnt > 0)
                            sqlQuery += ", ";
                        sqlQuery += " @ClassRole = '" + cp.ClassRole + "'";
                        paramCnt++;
                    }


                    var rdr = _context.Database.ExecuteSqlQuery(sqlQuery);
                    rdr.DbDataReader.Dispose();
                    updateResp.success = true;
                }
            }
            catch (Exception ex)
            {
                updateResp.success = false;
                _context.Database.ExecuteSqlQuery(@"
				exec [LOGS].[SP_LogApplicationMsg] @Type='ERROR', @Message='" + ex.Message + "', @Module='SSClassRepository.AddMember'");
            }

            return updateResp;

        }

        public UpdateResponse RemoveMember(string baseURL, ClassPerson cp)
        {
            //----------------------------------------------------------
            //-- Michael Yaacoub - 10/06/2017
            //-- csdmichael@gmail.com
            //----------------------------------------------------------
            UpdateResponse updateResp = new UpdateResponse();

            try
            {
                AuthResponse auth = _tokenInfoRepository.VerifyToken(cp.Token, "RemoveMember", "");
                updateResp.authResponse = auth;

                if (auth.success)
                {
                    string sqlQuery;

                    sqlQuery = @"exec SDS.[SP_Class_RemoveMember]";
                    int paramCnt = 0;

                    if (cp.ClassID != null && cp.ClassID.Trim() != "")
                    {
                        if (paramCnt > 0)
                            sqlQuery += ", ";
                        sqlQuery += " @ClassID = '" + cp.ClassID + "'";
                        paramCnt++;
                    }

                    if (cp.PersonID != null && cp.PersonID.Trim() != "")
                    {
                        if (paramCnt > 0)
                            sqlQuery += ", ";
                        sqlQuery += " @PersonID = '" + cp.PersonID + "'";
                        paramCnt++;
                    }

                    
                    var rdr = _context.Database.ExecuteSqlQuery(sqlQuery);
                    rdr.DbDataReader.Dispose();
                    updateResp.success = true;
                }
            }
            catch (Exception ex)
            {
                updateResp.success = false;
                _context.Database.ExecuteSqlQuery(@"
				exec [LOGS].[SP_LogApplicationMsg] @Type='ERROR', @Message='" + ex.Message + "', @Module='SSClassRepository.RemoveMember'");
            }

            return updateResp;

        }

        public ClassesResponse GetClassesByChurch(string baseURL, string token, string churchID, string classID)
        {
            //----------------------------------------------------------
            //-- Michael Yaacoub - 10/06/2017
            //-- csdmichael@gmail.com
            //----------------------------------------------------------
            ClassesResponse classesResp = new ClassesResponse();
            var lstClasses = new List<SSClass>();

            try
            {
                AuthResponse auth = _tokenInfoRepository.VerifyToken(token, "GetClassesByChurch", "");
                classesResp.authResponse = auth;

                if (auth.success)
                {
                    string sqlQuery;

                    sqlQuery = @"exec SDS.[SP_ClassesByChurch_Get]";
                    int paramCnt = 0;

                    if (churchID != null && churchID.Trim() != "")
                    {
                        if (paramCnt > 0)
                            sqlQuery += ", ";
                        sqlQuery += " @ChurchID = '" + churchID + "'";
                        paramCnt++;
                    }

                    if (classID != null && classID.Trim() != "")
                    {
                        if (paramCnt > 0)
                            sqlQuery += ", ";
                        sqlQuery += " @ClassID = '" + classID + "'";
                        paramCnt++;
                    }

                    var rdr = _context.Database.ExecuteSqlQuery(sqlQuery);

                    while (rdr.DbDataReader.Read())
                    {
                        SSClass cl = new SSClass();

                        cl.ClassID = System.Convert.ToInt32(rdr.DbDataReader["ClassID"]);
                        cl.ChurchID = rdr.DbDataReader["ChurchID"].ToString();
                        cl.ClassName = rdr.DbDataReader["ClassName"].ToString();
                        cl.StartDate = rdr.DbDataReader["StartDate"].ToString();
                        cl.FinishDate = rdr.DbDataReader["FinishDate"].ToString();
                        cl.Grade = rdr.DbDataReader["Grade"].ToString();
                        cl.Sex = rdr.DbDataReader["Sex"].ToString();
                        cl.LessonsCount = System.Convert.ToInt32(rdr.DbDataReader["LessonsCount"]);
                        cl.AttendCount = System.Convert.ToInt32(rdr.DbDataReader["AttendCount"]);
                        cl.TotalCount = System.Convert.ToInt32(rdr.DbDataReader["TotalCount"]);
                        cl.AttendPerc = System.Convert.ToInt32(rdr.DbDataReader["AttendPerc"]);
                        cl.SaintID = rdr.DbDataReader["SaintID"].ToString();

                        lstClasses.Add(cl);
                    }
                    rdr.DbDataReader.Dispose();
                }
            }
            catch (Exception ex)
            {
                _context.Database.ExecuteSqlQuery(@"
				exec [LOGS].[SP_LogApplicationMsg] @Type='ERROR', @Message='" + ex.Message + "', @Module='SSClassRepository.GetClassesByChurch'");
            }
            classesResp.ClassesList = lstClasses;
            classesResp.ClassesListCount = lstClasses.Count;
            return classesResp;
        }

        public PersonResponse GetServantsByClass(string baseURL, string token, string classID)
        {
            //----------------------------------------------------------
            //-- Michael Yaacoub - 10/06/2017
            //-- csdmichael@gmail.com
            //----------------------------------------------------------
            PersonResponse pResp = new PersonResponse();

            var lstervants = new List<Person>();

            try
            {
                AuthResponse auth = _tokenInfoRepository.VerifyToken(token, "GetServantsByClass", "");
                pResp.authResponse = auth;

                if (auth.success)
                {
                    string sqlQuery;

                    sqlQuery = @"exec SDS.[SP_ServantsByClass_Get]";
                    int paramCnt = 0;

                    if (classID != null && classID.Trim() != "")
                    {
                        sqlQuery += " @ClassID = '" + classID + "'";
                        paramCnt++;
                    }

                    var rdr = _context.Database.ExecuteSqlQuery(sqlQuery);

                    while (rdr.DbDataReader.Read())
                    {
                        Person p = new Person();

                        p.ClassID = classID.ToString();
                        p.PersonID = rdr.DbDataReader["PersonID"].ToString();
                        p.PersonName = rdr.DbDataReader["PersonName"].ToString();
                        p.CellPhone = rdr.DbDataReader["CellPhone"].ToString();
                        p.Email = rdr.DbDataReader["Email"].ToString();
                        p.DOB = rdr.DbDataReader["DOB"].ToString();
                        p.FirstName = rdr.DbDataReader["FirstName"].ToString();
                        p.LastName = rdr.DbDataReader["LastName"].ToString();
                        p.PictureName = rdr.DbDataReader["PictureName"].ToString();

                        lstervants.Add(p);
                    }
                    rdr.DbDataReader.Dispose();
                }
            }
            catch (Exception ex)
            {
                _context.Database.ExecuteSqlQuery(@"
				exec [LOGS].[SP_LogApplicationMsg] @Type='ERROR', @Message='" + ex.Message + "', @Module='SSClassRepository.GetServantsByClass'");
            }
            pResp.PersonList = lstervants;
            pResp.PersonListCount = lstervants.Count;

            return pResp;
        }
    }
}
