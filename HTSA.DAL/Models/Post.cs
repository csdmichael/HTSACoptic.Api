//----------------------------------------------------------
//-- Author: Michael Yaacoub
//-- Email: csdmichael@gmail.com
//----------------------------------------------------------
namespace HTSA.Models
{

    public class Post
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
        public int PostID { get; set; }
        public string PersonID { get; set; }
        public string PersonName { get; set; }
        public string PostBody { get; set; }
        public string PostTime { get; set; }
        public string PostImageName { get; set; }
        public string PictureName { get; set; }
        public string ChurchID { get; set; }

    }
}
