//----------------------------------------------------------
//-- Author: Michael Yaacoub
//-- Email: csdmichael@gmail.com
//----------------------------------------------------------
using System.Collections.Generic;

namespace HTSA.Models
{

    public class BibleChapter
    {
        public string ChapterNum { get; set; }
        public List<BibleVerse> VersesList { get; set; }

        public BibleChapter()
        {
            VersesList = new List<BibleVerse>();
        }
    }
}
