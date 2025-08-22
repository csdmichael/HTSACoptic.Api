using HTSA.Models;
//----------------------------------------------------------
//-- Author: Michael Yaacoub
//-- Email: csdmichael@gmail.com
//----------------------------------------------------------

namespace HTSA.Repositories
{
    public interface ISSClassRepository
    {

        ClassesResponse GetClassesByChurch(string baseURL, string token, string churchID, string classID);
        PersonResponse GetServantsByClass(string baseURL, string token, string classID);
        UpdateResponse AddClass(string baseURL, SSClass ssClass);
        UpdateResponse DeleteClass(string baseURL, SSClass ssClass);
        UpdateResponse RemoveMember(string baseURL, ClassPerson cp);
        UpdateResponse AddMember(string baseURL, ClassPerson cp);
    }
}