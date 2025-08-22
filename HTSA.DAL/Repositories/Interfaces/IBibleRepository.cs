using HTSA.Models;
//----------------------------------------------------------
//-- Author: Michael Yaacoub
//-- Email: csdmichael@gmail.com
//----------------------------------------------------------

namespace HTSA.Repositories
{
    public interface IBibleRepository
    {
        BibleResponse GetBooks(string baseURL, string token, string Keyword, string BookID, string BookName, string BookCode, string Testament);
        BibleResponse GetChapters(string baseURL, string token, string Keyword, string BookID, string BookName, string BookCode, string Testament, string ChapterNum);
        BibleResponse GetVerses(string baseURL, string token, string Keyword, string BookID, string BookName, string BookCode, string Testament, string ChapterNum, string VerseID);
        BibleResponse GetVersesWithFavs(string baseURL, string token, string Keyword, string BookID, string BookName, string BookCode, string Testament, string ChapterNum, string VerseID, string personId);
        BibleHierarchy GetBibleHierarchy();
    }
}