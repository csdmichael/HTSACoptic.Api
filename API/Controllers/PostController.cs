using Microsoft.AspNetCore.Mvc;
using HTSA.Repositories;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using TokenAuth.Models;
using HTSA.Models;
using TokenAuth.Repositories;
//----------------------------------------------------------
//-- Author: Michael Yaacoub
//-- Email: csdmichael@gmail.com
//----------------------------------------------------------

namespace HTSA.Controllers
{
    //[Authorize]
    [Route("[controller]")]
    public class PostController : Controller
    {
        IPostRepository _PostRepository;
        readonly ILogger<PostController> _logger;
        IAuthRepository _authRepository;

        public PostController(ILogger<PostController> logger, IPostRepository PostRepository, IAuthRepository authRepository)
        {
            _PostRepository = PostRepository;
            _logger = logger;
            _authRepository = authRepository;
        }
        
        //----------------------------------------------------------
        //-- Michael Yaacoub - 10/7/2017
        //-- csdmichael@gmail.com
        //----------------------------------------------------------

        [HttpGet("getposts")]
        public string GetPosts(
            /*string token, */
             string churchID)
        {
           
            string baseURL = Request.Scheme + "://" + Request.Host.Value;

            var result = _PostRepository.GetPosts(baseURL, /*token,*/ churchID);
            return JsonConvert.SerializeObject(result);
            
        }

        [Authorize()]
        [HttpPost("update")]
        public async Task<IActionResult> UpdatePost([FromBody] Post post)
        {
            string baseURL = Request.Scheme + "://" + Request.Host.Value;
            TokenPayLoad tokenPayload;
            tokenPayload = _authRepository.DecodeToken(this.Request.Headers["Authorization"]);

            if (tokenPayload.Role == "InactiveUser")
                return Unauthorized("Only Active users can call this method!");

            var result = _PostRepository.UpdatePost(baseURL, post.PostID, tokenPayload.PersonId, post.PostBody, post.PostImageName, tokenPayload.DisplayName);
            return Ok(JsonConvert.SerializeObject(result));
        }

        [Authorize()]
        [HttpPost("add")]
        public async Task<IActionResult> AddPost([FromBody] Post post)
        {
            string baseURL = Request.Scheme + "://" + Request.Host.Value;
            TokenPayLoad tokenPayload;
            tokenPayload = _authRepository.DecodeToken(this.Request.Headers["Authorization"]);

            if (tokenPayload.Role == "InactiveUser")
                return Unauthorized("Only Active users can call this method!");

            var result = _PostRepository.AddPost(baseURL, post.ChurchID, tokenPayload.PersonId, post.PostBody, post.PostImageName, tokenPayload.DisplayName);
            return Ok(JsonConvert.SerializeObject(result));
        }

        [Authorize()]
        [HttpPost("delete")]
        public async Task<IActionResult> DeletePost([FromBody] Post post)
        {
            string baseURL = Request.Scheme + "://" + Request.Host.Value;
            TokenPayLoad tokenPayload;
            tokenPayload = _authRepository.DecodeToken(this.Request.Headers["Authorization"]);

            if (tokenPayload.Role == "InactiveUser")
                return Unauthorized("Only Active users can call this method!");

            var result = _PostRepository.DeletePost(baseURL, post.PostID, tokenPayload.PersonId);
            return Ok(JsonConvert.SerializeObject(result));
        }

    }
}
