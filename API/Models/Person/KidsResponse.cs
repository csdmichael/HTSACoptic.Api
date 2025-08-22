using System.Collections.Generic;
//----------------------------------------------------------
//-- Author: Michael Yaacoub
//-- Email: csdmichael@gmail.com
//----------------------------------------------------------
namespace HTSA.Models
{
    public class KidsResponse
    {
        public string parentURL { get; set; }
        public int KidsListCount { get; set; }
        public List<Kid> KidsList { get; set; }

        

    }


}
