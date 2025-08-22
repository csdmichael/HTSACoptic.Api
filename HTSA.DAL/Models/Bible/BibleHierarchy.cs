//----------------------------------------------------------
//-- Author: Michael Yaacoub
//-- Email: csdmichael@gmail.com
//----------------------------------------------------------
using System.Collections.Generic;

namespace HTSA.Models
{

    public class BibleHierarchy
    {
        public List<BibleBook> BooksList { get; set; }

        public BibleHierarchy()
        {
            this.BooksList = new List<BibleBook>();
        }

    }
}
