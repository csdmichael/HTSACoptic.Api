using HTSA.Models;
//----------------------------------------------------------
//-- Author: Michael Yaacoub
//-- Email: csdmichael@gmail.com
//----------------------------------------------------------

namespace HTSA.Repositories
{
    public interface IAppRepository
    {

        AppVersionsResponse GetAppVersions(string baseURL);
        string GetCurrentVersion(string baseURL);
    }
}