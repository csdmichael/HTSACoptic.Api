using System.Collections.Generic;
//----------------------------------------------------------
//-- Author: Michael Yaacoub
//-- Email: csdmichael@gmail.com
//----------------------------------------------------------
namespace HTSA.Models
{
    public class LoginDetailsResponse
    {
        // public AuthResponse authResponse { get; set; }
        public string parentURL { get; set; }
        public int LoginDetailsListCount { get; set; }
        public List<LoginDetailsInfo> LoginDetailsList { get; set; }

        

    }


}
