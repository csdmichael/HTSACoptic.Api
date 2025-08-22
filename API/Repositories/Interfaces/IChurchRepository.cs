using HTSA.API.Models;
using HTSA.Models;
using System.Threading.Tasks;
//----------------------------------------------------------
//-- Author: Michael Yaacoub
//-- Email: csdmichael@gmail.com
//----------------------------------------------------------

namespace HTSA.Repositories
{
    public interface IChurchRepository
    {

        ChurchesResponse GetChurches(string baseURL, string token, string churchID);
        PersonChurchesResponse GetPersonChurches(string baseURL, string PersonID);

        UpdateResponse AddPersonChurch(PersonChurch pc);
        UpdateResponse UpdatePersonChurch(PersonChurch pc);
        Task<PeopleResponse> GetMembers(string churchId, string statusId, string baseURL);

        PeopleResponse GetAppAdmins();
        PeopleResponse GetChurchAdmins(string churchId);
    }
}