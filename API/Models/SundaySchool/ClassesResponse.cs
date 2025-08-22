using System.Collections.Generic;
//----------------------------------------------------------
//-- Author: Michael Yaacoub
//-- Email: csdmichael@gmail.com
//----------------------------------------------------------
namespace HTSA.Models
{
    public class ClassesResponse
    {
        public string parentURL { get; set; }
        public int ClassesListCount { get; set; }
        public List<SSClass> ClassesList { get; set; }

        

    }


}
