//----------------------------------------------------------
//-- Author: Michael Yaacoub
//-- Email: csdmichael@gmail.com
//----------------------------------------------------------
namespace HTSA.Models
{
    public class PersonPrivSettingsUpdateResponse
    {
        public string Token { get; set; }

        public string PersonID { get; set; }
        public string PrivacySettingID { get; set; }
        public string PrivacyLevelID { get; set; }

}


}
