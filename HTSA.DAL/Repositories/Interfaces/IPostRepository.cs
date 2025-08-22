using HTSA.DAL.Models;
using HTSA.Models;
using System.Threading.Tasks;
//----------------------------------------------------------
//-- Author: Michael Yaacoub
//-- Email: csdmichael@gmail.com
//----------------------------------------------------------

namespace HTSA.Repositories
{
    public interface IPostRepository
    {
        
        PostsResponse GetPosts(string baseURL, /*string token,*/ string churchID);
        Task<BasicReponse> AddPost(string baseURL, string churchID, string personId, string postBody, string postImageName, string personName);
        Task<BasicReponse> UpdatePost(string baseURL, int postId, string personId, string postBody, string postImageName, string personName);
        Task<BasicReponse> DeletePost(string baseURL, int postId, string personId);
    }
}