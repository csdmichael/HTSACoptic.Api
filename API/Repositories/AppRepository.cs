using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using HTSA.Models;
using System;
using TokenAuth.Repositories;
using HTSA.API.Models;
//----------------------------------------------------------
//-- Author: Michael Yaacoub
//-- Email: csdmichael@gmail.com
//----------------------------------------------------------

namespace HTSA.Repositories
{
    public class AppRepository : IAppRepository
    {
        ILogger _logger;
        ApplContext _context;

        public AppRepository(ApplContext context, ILogger<PostRepository> logger)
        {
            _logger = logger;
            _context = context;
        }

        public AppVersionsResponse GetAppVersions(string baseURL)
        {
            //----------------------------------------------------------
            //-- Michael Yaacoub - 10/06/2017
            //-- csdmichael@gmail.com
            //----------------------------------------------------------
            AppVersionsResponse appVersionsResp = new AppVersionsResponse();
            var lstAppVersions = new List<AppVersion>();

            try
            {
                
               
                string sqlQuery;

                sqlQuery = @"exec dbo.SP_App_Versions_Get";
                
                var rdr = _context.Database.ExecuteSqlQuery(sqlQuery);

                while (rdr.DbDataReader.Read())
                {
                    AppVersion appVersion = new AppVersion();

                    appVersion.VersionId = rdr.DbDataReader["VersionId"].ToString();
                    appVersion.ReleaseDate = rdr.DbDataReader["ReleaseDate"].ToString();
                    appVersion.UpgradeAction = rdr.DbDataReader["UpgradeAction"].ToString();
                    appVersion.IsLatest = System.Convert.ToInt32(rdr.DbDataReader["IsLatest"].ToString());
                    appVersion.FeatureId = System.Convert.ToInt32(rdr.DbDataReader["FeatureId"].ToString());
                    appVersion.Title = rdr.DbDataReader["Title"].ToString();
                    appVersion.Description = rdr.DbDataReader["Description"].ToString();


                    lstAppVersions.Add(appVersion);
                }
                rdr.DbDataReader.Dispose();
                //}
            }
            catch (Exception ex)
            {
                _context.Database.ExecuteSqlQuery(@"
				exec [LOGS].[SP_LogApplicationMsg] @Type='ERROR', @Message='" + ex.Message + "', @Module='AppRepository.GetAppVersions'");
            }
            appVersionsResp.AppVersionsList = lstAppVersions;
            return appVersionsResp;
        }

        public string GetCurrentVersion(string baseURL)
        {
            string currentVersion = "";
            AppVersionsResponse appVers = this.GetAppVersions(baseURL);

            foreach(AppVersion ver in appVers.AppVersionsList)
            {
                if (ver.IsLatest == 1)
                {
                    currentVersion = ver.VersionId;
                    break;
                }
            }

            return currentVersion;
        }
    }
}
