using System.Collections.Generic;
//----------------------------------------------------------
//-- Author: Michael Yaacoub
//-- Email: csdmichael@gmail.com
//----------------------------------------------------------
namespace HTSA.Models
{
    public class BibleResponse
    {
        public string parentURL { get; set; }
        public int BibleObjsListCount { get; set; }
        public List<BibleObj> BibleObjsList { get; set; }

        

    }


}
