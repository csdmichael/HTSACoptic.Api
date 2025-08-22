using System.Collections.Generic;
//----------------------------------------------------------
//-- Author: Michael Yaacoub
//-- Email: csdmichael@gmail.com
//----------------------------------------------------------
namespace HTSA.Models
{
    public class PrivacySettingsResponse
    {
        public string parentURL { get; set; }
        public int PrivacyLevelsListCount { get; set; }
        public List<PrivacyLevel> PrivacyLevelsList { get; set; }
        public int PrivacySettingsListCount { get; set; }
        public List<PrivacySetting> PrivacySettingsList { get; set; }

    }


}
