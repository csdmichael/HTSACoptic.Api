using System.Collections.Generic;
//----------------------------------------------------------
//-- Author: Michael Yaacoub
//-- Email: csdmichael@gmail.com
//----------------------------------------------------------
namespace HTSA.Models
{
    public class PersonPrivacySettingsResponse
    {
        public string parentURL { get; set; }
        public int PersonPrivacySettingsListCount { get; set; }
        public List<PersonPrivacySetting> PersonPrivacySettingsList { get; set; }
        

    }


}
