using Microsoft.AspNetCore.Mvc;
using HTSA.Repositories;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using TokenAuth.Repositories;
using Microsoft.AspNetCore.Authorization;
using TokenAuth.Models;
//----------------------------------------------------------
//-- Author: Michael Yaacoub
//-- Email: csdmichael@gmail.com
//----------------------------------------------------------

namespace HTSA.Controllers
{
    //[Authorize]
    [Route("[controller]")]
    public class BibleController : Controller
    {
        IBibleRepository _bibleRepository;
        readonly ILogger<BibleController> _logger;
        IAuthRepository _authRepository;

        public BibleController(ILogger<BibleController> logger, IBibleRepository BibleRepository, IAuthRepository authRepository)
        {
            _bibleRepository = BibleRepository;
            _logger = logger;
            _authRepository = authRepository;
        }
        
        //----------------------------------------------------------
        //-- Michael Yaacoub - 10/7/2017
        //-- csdmichael@gmail.com
        //----------------------------------------------------------

        [HttpGet("GetBooks")]
        public string GetBooks(string token, string Keyword, string BookID, string BookName, string BookCode, string Testament)
        {
           
            string baseURL = Request.Scheme + "://" + Request.Host.Value;

            var result = _bibleRepository.GetBooks(baseURL, token, Keyword, BookID, BookName, BookCode, Testament);
            return JsonConvert.SerializeObject(result);
            
        }

        [HttpGet("GetChapters")]
        public string GetChapters(string token, string Keyword, string BookID, string BookName, string BookCode, string Testament, string ChapterNum)
        {

            string baseURL = Request.Scheme + "://" + Request.Host.Value;

            var result = _bibleRepository.GetChapters(baseURL, token, Keyword, BookID, BookName, BookCode, Testament, ChapterNum);
            return JsonConvert.SerializeObject(result);

        }

        [HttpGet("GetVerses")]
        public string GetVerses(string token, string Keyword, string BookID, string BookName, string BookCode, string Testament, string ChapterNum, string VerseID)
        {

            string baseURL = Request.Scheme + "://" + Request.Host.Value;

            var result = _bibleRepository.GetVerses(baseURL, token, Keyword, BookID, BookName, BookCode, Testament, ChapterNum, VerseID);
            return JsonConvert.SerializeObject(result);

        }

        [Authorize()]
        [HttpGet("verses")]
        public string GetVersesWithFavs(string token, string Keyword, string BookID, string BookName, string BookCode, string Testament, string ChapterNum, string VerseID)
        {

            string baseURL = Request.Scheme + "://" + Request.Host.Value;

            TokenPayLoad tokenPayload;
            tokenPayload = _authRepository.DecodeToken(this.Request.Headers["Authorization"]);

            var result = _bibleRepository.GetVersesWithFavs(baseURL, token, Keyword, BookID, BookName, BookCode, Testament, ChapterNum, VerseID, tokenPayload.PersonId);
            return JsonConvert.SerializeObject(result);

        }

    }
}
