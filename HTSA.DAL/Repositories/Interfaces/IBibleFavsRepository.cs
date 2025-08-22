using HTSA.DAL.Models;
using HTSA.Models;
using System.Threading.Tasks;
//----------------------------------------------------------
//-- Author: Michael Yaacoub
//-- Email: csdmichael@gmail.com
//----------------------------------------------------------

namespace HTSA.Repositories
{
    public interface IBibleFavsRepository
    {
        
        FavVersesResponse GetMyFavVerses(string baseURL, string personId);
        FavVersesResponse GetAllFavVerses(string baseURL);
        Task<BasicReponse> AddVerseToFavorites(string baseURL, string verseId, string personId);
        Task<BasicReponse> RemoveVerseFromFavorites(string baseURL, string verseId, string personId);
        BasicReponse IncrementSentCount(string baseURL, string verseId);
    }
}