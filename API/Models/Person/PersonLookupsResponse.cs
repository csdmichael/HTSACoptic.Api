using System.Collections.Generic;
//----------------------------------------------------------
//-- Author: Michael Yaacoub
//-- Email: csdmichael@gmail.com
//----------------------------------------------------------
namespace HTSA.Models
{
    public class PersonLookupsResponse
    {
        public string parentURL { get; set; }
        public int LocCountryListCount { get; set; }
        public List<LocCountry> LocCountryList { get; set; }
        public int LocStateListCount { get; set; }
        public List<LocState> LocStateList { get; set; }
        public int LocCityListCount { get; set; }
        public List<LocCity> LocCityList { get; set; }
		public int MartialStatusListCount { get; set; }
        public List<MartialStatus> MartialStatusList { get; set; }
        public int ChurchesListCount { get; set; }
        public List<Church> ChurchesList { get; set; }



    }


}
