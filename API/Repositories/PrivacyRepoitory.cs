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
    public class PrivacyRepository : IPrivacyRepository
    {
        ILogger _logger;
        ApplContext _context;
        ITokenInfoRepository _tokenInfoRepository;

        public PrivacyRepository(ApplContext context, ILogger<PrivacyRepository> logger, ITokenInfoRepository tokenInfoRepository)
        {
            _logger = logger;
            _context = context;
            _tokenInfoRepository = tokenInfoRepository;
        }

       
        public PrivacySettingsResponse GetLookups(string baseURL, string token)
        {
            //----------------------------------------------------------
            //-- Michael Yaacoub - 10/06/2017
            //-- csdmichael@gmail.com
            //----------------------------------------------------------
            PrivacySettingsResponse privacySettingsResp = new PrivacySettingsResponse();
            var lstPrivacyLevels = new List<PrivacyLevel>();
			var lstPrivacySettings = new List<PrivacySetting>();

            try
            {
                AuthResponse auth = _tokenInfoRepository.VerifyToken(token, "GetLookups", "");
                privacySettingsResp.authResponse = auth;

                if (auth.success)
                {
                    string sqlQuery;

                    sqlQuery = @"exec PRIV.SP_PrivacySettings_GetLookups";
                    

                    var rdr = _context.Database.ExecuteSqlQuery(sqlQuery);

                    while (rdr.DbDataReader.Read())
                    {
                        PrivacyLevel pl = new PrivacyLevel();

						//Read fields from DataReader
						//-----------------------------
						pl.PrivacyLevelID = rdr.DbDataReader["PrivacyLevelID"].ToString();
						pl.PrivacyLevelName = rdr.DbDataReader["PrivacyLevelName"].ToString();
						lstPrivacyLevels.Add(pl);
                    }
					
					rdr.DbDataReader.NextResult();
					
					while (rdr.DbDataReader.Read())
                    {
                        PrivacySetting ps = new PrivacySetting();

						//Read fields from DataReader
						//-----------------------------
						ps.PrivacySettingID = rdr.DbDataReader["PrivacySettingID"].ToString();
						ps.PrivacySettingName = rdr.DbDataReader["PrivacySettingName"].ToString();
						ps.DefaultPrivacyLevelID = rdr.DbDataReader["DefaultPrivacyLevelID"].ToString();
						lstPrivacySettings.Add(ps);
                    }
					
									
                    rdr.DbDataReader.Dispose();
                }
            }
            catch (Exception ex)
            {
                _context.Database.ExecuteSqlQuery(@"
				exec [LOGS].[SP_LogApplicationMsg] @Type='ERROR', @Message='" + ex.Message + "', @Module='PrivacyRepository.GetLookups'");
            }
			
            privacySettingsResp.PrivacyLevelsList = lstPrivacyLevels;
            privacySettingsResp.PrivacyLevelsListCount = lstPrivacyLevels.Count;
			
			privacySettingsResp.PrivacySettingsList = lstPrivacySettings;
            privacySettingsResp.PrivacySettingsListCount = lstPrivacySettings.Count;
			
            return privacySettingsResp;
        }

        public PersonPrivacySettingsResponse GetPersonPrivSettings(string baseURL, string token, string PersonID, string PrivacySettingID)
        {
            //----------------------------------------------------------
            //-- Michael Yaacoub - 10/06/2017
            //-- csdmichael@gmail.com
            //----------------------------------------------------------
            PersonPrivacySettingsResponse personPrivacySettingsResponse = new PersonPrivacySettingsResponse();
            var lstPersonPrivacySettings = new List<PersonPrivacySetting>();

            try
            {
                AuthResponse auth = _tokenInfoRepository.VerifyToken(token, "GetPersonPrivSettings", "");
                personPrivacySettingsResponse.authResponse = auth;

                if (auth.success)
                {
                    string sqlQuery;
					int paramCnt = 0;

                    sqlQuery = @"exec PRIV.SP_Person_PrivacySettings_Get";
					
					if (PersonID != null && PersonID.Trim() != "")
                    {
                        if (paramCnt > 0)
                            sqlQuery += ", ";
                        sqlQuery += " @PersonID = '" + PersonID + "'";
                        paramCnt++;
                    }
					
					if (PrivacySettingID != null && PrivacySettingID.Trim() != "")
                    {
                        if (paramCnt > 0)
                            sqlQuery += ", ";
                        sqlQuery += " @PrivacySettingID = '" + PrivacySettingID + "'";
                        paramCnt++;
                    }
                    

                    var rdr = _context.Database.ExecuteSqlQuery(sqlQuery);

                    while (rdr.DbDataReader.Read())
                    {
                        PersonPrivacySetting pps = new PersonPrivacySetting();

						//Read fields from DataReader
						//-----------------------------
                        pps.PersonID = rdr.DbDataReader["PersonID"].ToString();
						pps.PrivacySettingID = rdr.DbDataReader["PrivacySettingID"].ToString();
						pps.PrivacySettingName = rdr.DbDataReader["PrivacySettingName"].ToString();
						pps.PrivacyLevelID = rdr.DbDataReader["PrivacyLevelID"].ToString();
						pps.PrivacyLevelName = rdr.DbDataReader["PrivacyLevelName"].ToString();
						pps.PersonName = rdr.DbDataReader["PersonName"].ToString();


                        lstPersonPrivacySettings.Add(pps);
                    }
                    rdr.DbDataReader.Dispose();
                }
            }
            catch (Exception ex)
            {
                _context.Database.ExecuteSqlQuery(@"
				exec [LOGS].[SP_LogApplicationMsg] @Type='ERROR', @Message='" + ex.Message + "', @Module='PrivacyRepository.GetPersonPrivSettings'");
            }
			
            personPrivacySettingsResponse.PersonPrivacySettingsList = lstPersonPrivacySettings;
            personPrivacySettingsResponse.PersonPrivacySettingsListCount = lstPersonPrivacySettings.Count;
            return personPrivacySettingsResponse;
        }
		
		public PersonChurchRolesResponse GetPersonChurchRole(string baseURL, string token, string ChurchID, string ChurchRoleID, string PersonID)
        {
            //----------------------------------------------------------
            //-- Michael Yaacoub - 10/06/2017
            //-- csdmichael@gmail.com
            //----------------------------------------------------------
            PersonChurchRolesResponse personChurchRolesResp = new PersonChurchRolesResponse();
            var lstPersonChurchRoles = new List<PersonChurchRole>();

            try
            {
                AuthResponse auth = _tokenInfoRepository.VerifyToken(token, "GetPersonChurchRole", "");
                personChurchRolesResp.authResponse = auth;

                if (auth.success)
                {
                    string sqlQuery;
					int paramCnt = 0;

                    sqlQuery = @"exec PRIV.SP_Person_Church_Role_Get";
                    
					if (ChurchID != null && ChurchID.Trim() != "")
                    {
                        if (paramCnt > 0)
                            sqlQuery += ", ";
                        sqlQuery += " @ChurchID = '" + ChurchID + "'";
                        paramCnt++;
                    }
					
					if (ChurchRoleID != null && ChurchRoleID.Trim() != "")
                    {
                        if (paramCnt > 0)
                            sqlQuery += ", ";
                        sqlQuery += " @ChurchRoleID = '" + ChurchRoleID + "'";
                        paramCnt++;
                    }
					
					if (PersonID != null && PersonID.Trim() != "")
                    {
                        if (paramCnt > 0)
                            sqlQuery += ", ";
                        sqlQuery += " @PersonID = '" + PersonID + "'";
                        paramCnt++;
                    }

                    var rdr = _context.Database.ExecuteSqlQuery(sqlQuery);
					

                    while (rdr.DbDataReader.Read())
                    {
                        PersonChurchRole pcr = new PersonChurchRole();

						//Read fields from DataReader
						//-----------------------------
                        pcr.PersonID = rdr.DbDataReader["PersonID"].ToString();
						pcr.PersonName = rdr.DbDataReader["PersonName"].ToString();
						pcr.ChurchID = rdr.DbDataReader["ChurchID"].ToString();
						pcr.ChurchRoleID = rdr.DbDataReader["ChurchRoleID"].ToString();


                        lstPersonChurchRoles.Add(pcr);
                    }
                    rdr.DbDataReader.Dispose();
                }
            }
            catch (Exception ex)
            {
                _context.Database.ExecuteSqlQuery(@"
				exec [LOGS].[SP_LogApplicationMsg] @Type='ERROR', @Message='" + ex.Message + "', @Module='PrivacyRepository.GetPersonChurchRole'");
            }
            personChurchRolesResp.PersonChurchRolesList = lstPersonChurchRoles;
            personChurchRolesResp.PersonChurchRolesListCount = lstPersonChurchRoles.Count;
            return personChurchRolesResp;
        }
		
		public PersonModuleRolesResponse GetPersonModuleRole(string baseURL, string token, string ChurchID, string ModuleRoleID, string ModuleID, string PersonID)
        {
            //----------------------------------------------------------
            //-- Michael Yaacoub - 10/06/2017
            //-- csdmichael@gmail.com
            //----------------------------------------------------------
            PersonModuleRolesResponse personModuleRolesResp = new PersonModuleRolesResponse();
            var lstPersonModuleRoles = new List<PersonModuleRole>();

            try
            {
                AuthResponse auth = _tokenInfoRepository.VerifyToken(token, "GetPersonModuleRole", "");
                personModuleRolesResp.authResponse = auth;

                if (auth.success)
                {
                    string sqlQuery;
					int paramCnt = 0;

                    sqlQuery = @"exec PRIV.SP_Person_Module_Role_Get";
					
					if (ChurchID != null && ChurchID.Trim() != "")
                    {
                        if (paramCnt > 0)
                            sqlQuery += ", ";
                        sqlQuery += " @ChurchID = '" + ChurchID + "'";
                        paramCnt++;
                    }
					
					if (ModuleRoleID != null && ModuleRoleID.Trim() != "")
                    {
                        if (paramCnt > 0)
                            sqlQuery += ", ";
                        sqlQuery += " @ModuleRoleID = '" + ModuleRoleID + "'";
                        paramCnt++;
                    }
					
					if (ModuleID != null && ModuleID.Trim() != "")
                    {
                        if (paramCnt > 0)
                            sqlQuery += ", ";
                        sqlQuery += " @ModuleID = '" + ModuleID + "'";
                        paramCnt++;
                    }
					
					if (PersonID != null && PersonID.Trim() != "")
                    {
                        if (paramCnt > 0)
                            sqlQuery += ", ";
                        sqlQuery += " @PersonID = '" + PersonID + "'";
                        paramCnt++;
                    }
                    

                    var rdr = _context.Database.ExecuteSqlQuery(sqlQuery);

                    while (rdr.DbDataReader.Read())
                    {
                        PersonModuleRole pmr = new PersonModuleRole();

						//Read fields from DataReader
						//-----------------------------
                        pmr.PersonID = rdr.DbDataReader["PersonID"].ToString();
						pmr.PersonName = rdr.DbDataReader["PersonName"].ToString();
						pmr.ChurchID = rdr.DbDataReader["ChurchID"].ToString();
						pmr.ModuleID = rdr.DbDataReader["ModuleID"].ToString();
						pmr.ModuleRoleID = rdr.DbDataReader["ModuleRoleID"].ToString();


                        lstPersonModuleRoles.Add(pmr);
                    }
                    rdr.DbDataReader.Dispose();
                }
            }
            catch (Exception ex)
            {
                _context.Database.ExecuteSqlQuery(@"
				exec [LOGS].[SP_LogApplicationMsg] @Type='ERROR', @Message='" + ex.Message + "', @Module='PrivacyRepository.GetPersonModuleRole'");
            }
            personModuleRolesResp.PersonModuleRolesList = lstPersonModuleRoles;
            personModuleRolesResp.PersonModuleRolesListCount = lstPersonModuleRoles.Count;
            return personModuleRolesResp;
        }
		
		public UpdateResponse SetPersonPrivSettings(PersonPrivSettingsUpdateResponse ppsr)
        {
            
            //----------------------------------------------------------
            //-- Michael Yaacoub - 10/06/2017
            //-- csdmichael@gmail.com
            //----------------------------------------------------------
            UpdateResponse updateResp = new UpdateResponse();

            try
            {
                AuthResponse auth = _tokenInfoRepository.VerifyToken(ppsr.Token, "SetPersonPrivSettings", "");
                updateResp.authResponse = auth;

                if (auth.success)
                {
                    if (ppsr == null)
                    {
                        throw new ArgumentNullException(nameof(ppsr));
                    }

                    string sqlQuery;
					int paramCnt = 0;

                    sqlQuery = @"exec  PRIV.SP_Person_PrivacySettings_Set";
                    
					if (ppsr.PersonID != null && ppsr.PersonID.Trim() != "")
                    {
                        if (paramCnt > 0)
                            sqlQuery += ", ";
                        sqlQuery += " @PersonID = '" + ppsr.PersonID + "'";
                        paramCnt++;
                    }
					
					if (ppsr.PrivacySettingID != null && ppsr.PrivacySettingID.Trim() != "")
                    {
                        if (paramCnt > 0)
                            sqlQuery += ", ";
                        sqlQuery += " @PrivacySettingIDs = '" + ppsr.PrivacySettingID + "'";
                        paramCnt++;
                    }
					
					if (ppsr.PrivacyLevelID != null && ppsr.PrivacyLevelID.Trim() != "")
                    {
                        if (paramCnt > 0)
                            sqlQuery += ", ";
                        sqlQuery += " @PrivacyLevelIDs = '" + ppsr.PrivacyLevelID + "'";
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
				exec [LOGS].[SP_LogApplicationMsg] @Type='ERROR', @Message='" + ex.Message + "', @Module='PrivacyRepository.SetPersonPrivSettings'");
            }
            
            return updateResp;
        }

        public UpdateResponse SetPersonPrivSettings(string baseURL, string token, PersonPrivacySettingsResponse ppsr)
        {
            throw new NotImplementedException();
        }
    }
}
