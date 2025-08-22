using HTSA.Models;
//----------------------------------------------------------
//-- Author: Michael Yaacoub
//-- Email: csdmichael@gmail.com
//----------------------------------------------------------

namespace HTSA.Repositories
{
    public interface IPrivacyRepository
    {
        PrivacySettingsResponse GetLookups(string baseURL, string token);
        PersonPrivacySettingsResponse GetPersonPrivSettings(string baseURL, string token, string PersonID, string PrivacySettingID);
        PersonChurchRolesResponse GetPersonChurchRole(string baseURL, string token, string ChurchID, string ChurchRoleID, string PersonID);
        PersonModuleRolesResponse GetPersonModuleRole(string baseURL, string token, string ChurchID, string ModuleRoleID, string ModuleID, string PersonID);
        UpdateResponse SetPersonPrivSettings(PersonPrivSettingsUpdateResponse ppsr);
        
    }
}