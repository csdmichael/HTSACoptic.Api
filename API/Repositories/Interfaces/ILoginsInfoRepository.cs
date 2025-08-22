using HTSA.Models;
//----------------------------------------------------------
//-- Author: Michael Yaacoub
//-- Email: csdmichael@gmail.com
//----------------------------------------------------------

namespace HTSA.Repositories
{
    public interface ILoginsInfoRepository
    {

        LoginsByUserIDResponse GetLoginsByUserID(string baseURL, string token);
        LoginsByModuleResponse GetLoginsByModule(string baseURL, string token);
        LoginsByDayResponse GetLoginsByDay(string baseURL, string token);
        LoginDetailsResponse GetLoginDetails(string date, string module, string userID, string baseURL, string token);

    }
}