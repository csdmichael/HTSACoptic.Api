using System.Collections.Generic;
//----------------------------------------------------------
//-- Author: Michael Yaacoub
//-- Email: csdmichael@gmail.com
//----------------------------------------------------------
namespace HTSA.Models
{
    public class LoginsByUserIDResponse
    {
        public string parentURL { get; set; }
        public int LoginsByUserIDListCount { get; set; }
        public List<LoginsByUserIDInfo> LoginsByUserIDList { get; set; }

        

    }


}
