using System.Collections.Generic;
//----------------------------------------------------------
//-- Author: Michael Yaacoub
//-- Email: csdmichael@gmail.com
//----------------------------------------------------------
namespace HTSA.Models
{
    public class ChurchesResponse
    {
        public string parentURL { get; set; }
        public int ChurchesListCount { get; set; }
        public List<Church> ChurchesList { get; set; }

        

    }


}
