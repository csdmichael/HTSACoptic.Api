//----------------------------------------------------------
//-- Author: Michael Yaacoub
//-- Email: csdmichael@gmail.com
//----------------------------------------------------------
namespace HTSA.Models
{

    public class Car
    {
        /*
        "ChurchID":"USCTHMDN",
        "ClassID":"USCTHMDN-M-8",
		"ClassName":"St. Antonious",
		"Sex":"M",
		"StartDate":"2017-09-09",
		"FinishDate":"2018-09-09",
		"Grade":"8"
        */
        public bool CanEdit { get; set; }
        public string Token { get; set; }
        public int CarID { get; set; }
        public string PersonID { get; set; }
        public string PersonName { get; set; }
        public string MakeID { get; set; }
        public string MakeName { get; set; }
        public string ColorID { get; set; }
        public string ColorName { get; set; }
        public string StyleID { get; set; }
        public string StyleName { get; set; }
        public string ModelName { get; set; }
        public string PlateNo { get; set; }
        public string StateLicensed { get; set; }
        public string CountryLicensed { get; set; }
        public string ChurchID { get; set; }
        public string StickerNum { get; set; }

    }
}
