//----------------------------------------------------------
//-- Author: Michael Yaacoub
//-- Email: csdmichael@gmail.com
//----------------------------------------------------------
namespace HTSA.Models
{

    public class Church
    {
        /*
        "ChurchID":"USCTHMDN",
                "ChurchName":"Virgin Mary & Archangel Michael Coptic Orthodox Church",
                "CountryID":"USA",
                "StateID": "CT",
                "CityID":2,
                "ZipCode":"06514",
                "CAddress":"87 Benham St",
                "LocLatitude":"-72.926529",
                "LocLongitude":"41.364369"
        */
        public string ChurchID { get; set; }
        public string ChurchName { get; set; }
        public string CountryID { get; set; }
        public string StateID { get; set; }
        public string CityID { get; set; }
        public string CityName { get; set; }
        public string ZipCode { get; set; }
        public string CAddress { get; set; }
        public string LocLatitude { get; set; }
        public string LocLongitude { get; set; }
        public string ChurchInfoHtml { get; set; }
    }
}
