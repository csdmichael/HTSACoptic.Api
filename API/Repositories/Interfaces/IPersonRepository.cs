using HTSA.API.Models;
using HTSA.DAL.Models;
using HTSA.Models;
using System.Threading.Tasks;
using TokenAuth.Models;
//----------------------------------------------------------
//-- Author: Michael Yaacoub
//-- Email: csdmichael@gmail.com
//----------------------------------------------------------

namespace HTSA.Repositories
{
    public interface IPersonRepository
    {

        PersonResponse GetPerson(string baseURL, string token, string personID, string churchID, string MyFriends, string ChurchRoleID, string RequestorID,
            string isAddFriend, string isAddPriest, string isAddDeacon, string isAddChurchMember, string AddChurchID, string isAddClassMember, string AddClassID, string ClassRole);
        PersonResponse GetBirthdaysOfWeek(string baseURL, string token, string churchID, string classID, string nextSunday, string MonthOrWeek, string Month);
		PersonResponse PersonDeActivate(string baseURL, string token, string personID);
		PersonResponse PersonActivate(string baseURL, string token, string personID);
		PersonLookupsResponse GetLookups(string baseURL, string token);
        PersonRelationsResponse GetPersonRelations(string baseURL, string token, string PersonID, string Relation);
        RelationsResponse GetRelationLookup(string baseURL, string token);

        UpdateResponse AddPersonRelation(PersonRelation pr);
        UpdateResponse UpdatePersonRelation(PersonRelation pr);
        Task<PeopleResponse> GetPeople(string personId, int isActive, int isSaint, string searchKeyword, string baseURL);

        //------------------------------------------------------------------------------------------------------

        UpdateResponse AddPerson(string baseURL, RegisteredUser pers, string activationCode);
        BasicReponse UpdatePerson(string baseURL, RegisteredUser pers, string personId);

        Task<RegisterResponse> RegisterUser(RegisteredUser registeredUser, string baseUrl);
        Task<RegisterResponse> LoginUser(AuthRequest authRequest, string baseUrl);
        Task<BasicReponse> ResetPassword(string email, string baseUrl);
        Task<BasicReponse> VerifyPhone(string phone, string baseUrl);

        Task<BasicReponse> SendAccountActivationCode(string email, string baseUrl);
        Task<BasicReponse> ActivateAccount(string email, string activationCode, string baseUrl);
        Task<BasicReponse> DeleteAccount(string email, string baseUrl);
        Task<UserInfoResponse> GetUserDetails(string personId, string baseUrl);

        Task<AuthResponse> SetHasPic(int hasPic, string personId, string baseUrl, TokenPayLoad tokenPayload, string fileName);
        Task<BasicReponse> ChangePassword(PersonPasswordChange pc, string personId, string baseUrl);
        Task<RegisterResponse> UpdateUser(string personId, RegisteredUser registeredUser, string baseUrl);

    }
}