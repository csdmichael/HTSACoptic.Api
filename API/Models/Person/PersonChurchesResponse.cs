using System.Collections.Generic;
//----------------------------------------------------------
//-- Author: Michael Yaacoub
//-- Email: csdmichael@gmail.com
//----------------------------------------------------------
namespace HTSA.Models
{
    public class PersonChurchesResponse
    {
        public string parentURL { get; set; }
        // public int PersonChurchesListCount { get; set; }
        public List<PersonChurch> PersonChurchesList { get; set; }
        

    }


}
