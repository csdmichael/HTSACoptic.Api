using System.Collections.Generic;
//----------------------------------------------------------
//-- Author: Michael Yaacoub
//-- Email: csdmichael@gmail.com
//----------------------------------------------------------

namespace HTSA.Models
{
    public class LoginDetailsInfo
    {
        public int LoginAttemptID { get; set; }
        public string UserID { get; set; }
        public string Token { get; set; }
        public bool IsLoginSuccess { get; set; }
        public string Module { get; set; }   
        public string LoginDateTime { get; set; }
        public string Params { get; set; }
        public int ParamsListCount { get; set; }
        public List<ParamValue> ParamsList { get; set; }

    }


}
