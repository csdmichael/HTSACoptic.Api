using System.Collections.Generic;
//----------------------------------------------------------
//-- Author: Michael Yaacoub
//-- Email: csdmichael@gmail.com
//----------------------------------------------------------
namespace HTSA.Models
{
    public class CarLookupsResponse
    {
        public string parentURL { get; set; }
        public int CarColorListCount { get; set; }
        public List<CarColor> CarColorList { get; set; }
        public int CarMakeListCount { get; set; }
        public List<CarMake> CarMakeList { get; set; }
        public int CarStyleListCount { get; set; }
        public List<CarStyle> CarStyleList { get; set; }
        public int PeopleListCount { get; set; }
        public List<Person> PeopleList { get; set; }



    }


}
