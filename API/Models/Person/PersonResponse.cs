using System.Collections.Generic;
//----------------------------------------------------------
//-- Author: Michael Yaacoub
//-- Email: csdmichael@gmail.com
//----------------------------------------------------------
namespace HTSA.Models
{
    public class PersonResponse
    {
        public string parentURL { get; set; }
        public int PersonListCount { get; set; }
        public List<Person> PersonList { get; set; }

        

    }


}
