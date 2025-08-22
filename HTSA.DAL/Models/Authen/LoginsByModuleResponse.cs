using System.Collections.Generic;
//----------------------------------------------------------
//-- Author: Michael Yaacoub
//-- Email: csdmichael@gmail.com
//----------------------------------------------------------
namespace HTSA.Models
{
    public class LoginsByModuleResponse
    {
        public string parentURL { get; set; }
        public int LoginsByModuleListCount { get; set; }
        public List<LoginsByModuleInfo> LoginsByModuleList { get; set; }

        

    }


}
