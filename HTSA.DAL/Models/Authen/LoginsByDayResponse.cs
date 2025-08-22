using System.Collections.Generic;
//----------------------------------------------------------
//-- Author: Michael Yaacoub
//-- Email: csdmichael@gmail.com
//----------------------------------------------------------
namespace HTSA.Models
{
    public class LoginsByDayResponse
    {
        public string parentURL { get; set; }
        public int LoginsByDayListCount { get; set; }
        public List<LoginsByDayInfo> LoginsByDayList { get; set; }

        

    }


}
