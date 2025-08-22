//----------------------------------------------------------
//-- Author: Michael Yaacoub
//-- Email: csdmichael@gmail.com
//----------------------------------------------------------
namespace HTSA.Models
{

    public class SSClass
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
        public string Token { get; set; }
        public string ChurchID { get; set; }
        public int ClassID { get; set; }
        public string ClassName { get; set; }
        public string Sex { get; set; }
        public string StartDate { get; set; }
        public string FinishDate { get; set; }
        public string Grade { get; set; }
        public int LessonsCount { get; set; }
        public int AttendCount { get; set; }
        public int TotalCount { get; set; }
        public int AttendPerc { get; set; }
        public string SaintID { get; set; }
    }
}
