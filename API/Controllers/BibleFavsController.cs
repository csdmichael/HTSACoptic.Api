using Microsoft.AspNetCore.Mvc;
using HTSA.Repositories;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using HTSA.Models;
using TokenAuth.Repositories;
using System.Threading.Tasks;
using TokenAuth.Models;
//----------------------------------------------------------
//-- Author: Michael Yaacoub
//-- Email: csdmichael@gmail.com
//----------------------------------------------------------

namespace HTSA.Controllers
{
    [Route("api/[controller]")]
    public class BibleFavsController : Controller
    {
        IBibleFavsRepository _bibleFavsRepository;
        readonly ILogger<BibleController> _logger;
        IAuthRepository _authRepository;

        public BibleFavsController(ILogger<BibleController> logger, IBibleFavsRepository bibleFavsRepository, IAuthRepository authRepository)
        {
            _bibleFavsRepository = bibleFavsRepository;
            _logger = logger;
            _authRepository = authRepository;
        }
        
        //----------------------------------------------------------
        //-- Michael Yaacoub 
        //-- csdmichael@gmail.com
        //----------------------------------------------------------
        [Authorize()]
        [HttpPost("add")]
        public async Task<IActionResult> AddVerseToFavorites([FromBody] FavVerse favVerse)
        {
           
            string baseURL = Request.Scheme + "://" + Request.Host.Value;

            TokenPayLoad tokenPayload;
            tokenPayload = _authRepository.DecodeToken(this.Request.Headers["Authorization"]);

            if (tokenPayload.Role == "InactiveUser")
                return Unauthorized("Only Active users can call this method!");

            var result = await _bibleFavsRepository.AddVerseToFavorites(baseURL, favVerse.VerseID, tokenPayload.PersonId);
            return Ok(JsonConvert.SerializeObject(result));
            
        }

        [Authorize()]
        [HttpPost("delete")]
        public async Task<IActionResult> DeleteVerseFromFavorites([FromBody] FavVerse favVerse)
        {

            string baseURL = Request.Scheme + "://" + Request.Host.Value;

            TokenPayLoad tokenPayload;
            tokenPayload = _authRepository.DecodeToken(this.Request.Headers["Authorization"]);

            if (tokenPayload.Role == "InactiveUser")
                return Unauthorized("Only Active users can call this method!");

            var result = await _bibleFavsRepository.RemoveVerseFromFavorites(baseURL, favVerse.VerseID, tokenPayload.PersonId);
            return Ok(JsonConvert.SerializeObject(result));

        }

        [Authorize()]
        [HttpGet("myverses")]
        public async Task<IActionResult> GetMyFavVerses()
        {
            string baseURL = Request.Scheme + "://" + Request.Host.Value;

            TokenPayLoad tokenPayload;
            tokenPayload = _authRepository.DecodeToken(this.Request.Headers["Authorization"]);

            if (tokenPayload.Role == "InactiveUser")
                return Unauthorized("Only Active users can call this method!");

            var result = _bibleFavsRepository.GetMyFavVerses(baseURL, tokenPayload.PersonId);
            return Ok(JsonConvert.SerializeObject(result));
        }

        [Authorize()]
        [HttpGet("allverses")]
        public async Task<IActionResult> GetAllFavVerses()
        {
            string baseURL = Request.Scheme + "://" + Request.Host.Value;

            TokenPayLoad tokenPayload;
            tokenPayload = _authRepository.DecodeToken(this.Request.Headers["Authorization"]);

            if (tokenPayload.Role == "InactiveUser")
                return Unauthorized("Only Active users can call this method!");

            var result = _bibleFavsRepository.GetAllFavVerses(baseURL);
            return Ok(JsonConvert.SerializeObject(result));
        }
    }
}
