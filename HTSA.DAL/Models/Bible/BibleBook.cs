//----------------------------------------------------------
//-- Author: Michael Yaacoub
//-- Email: csdmichael@gmail.com
//----------------------------------------------------------
using System.Collections.Generic;

namespace HTSA.Models
{

    public class BibleBook
    {
        public string Testament { get; set; }
        public string TestamentArabic { get; set; }
        public string BookName { get; set; }
        public string BookNameArabic { get; set; }
        public string BookCode { get; set; }
        public string BookID { get; set; }
        public List<BibleChapter> ChaptersList { get; set; }

        public BibleBook()
        {
            this.ChaptersList = new List<BibleChapter>();
        }

    }
}
