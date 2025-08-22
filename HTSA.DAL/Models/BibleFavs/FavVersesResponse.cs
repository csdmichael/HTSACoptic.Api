using System.Collections.Generic;
//----------------------------------------------------------
//-- Author: Michael Yaacoub
//-- Email: csdmichael@gmail.com
//----------------------------------------------------------
namespace HTSA.Models
{
    public class FavVersesResponse
    {
        public string parentURL { get; set; }
        public List<FavVerse> FavVersesList { get; set; }
    }


}
