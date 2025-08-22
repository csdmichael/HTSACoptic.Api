using HTSA.API.Models;
using System.Collections.Generic;
//----------------------------------------------------------
//-- Author: Michael Yaacoub
//-- Email: csdmichael@gmail.com
//----------------------------------------------------------
namespace HTSA.Models
{
    public class AppVersionsResponse
    {
        public List<AppVersion> AppVersionsList { get; set; }

        
        public AppVersionsResponse()
        {
            AppVersionsList = new List<AppVersion>();
        }
    }


}
