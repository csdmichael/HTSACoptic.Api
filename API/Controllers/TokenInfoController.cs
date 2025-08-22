using Microsoft.AspNetCore.Mvc;
using HTSA.Repositories;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using HTSA.Models;

namespace HTSA.Controllers
{
    //[Authorize]
    //[Route("api/[controller]")]
    public class TokenInfoController : Controller
    {
        readonly ILogger<TokenInfoController> _logger;
        ITokenInfoRepository _tokenInfoRepository;

        public class AuthRequest
        {
            public string username { get; set; }
            public string password { get; set; }
        }

        public TokenInfoController(ILogger<TokenInfoController> logger, ITokenInfoRepository tokenInfoRepository)
        {
            _logger = logger;
            _tokenInfoRepository = tokenInfoRepository;

        }

        [HttpPost]
        public dynamic Post([FromBody]AuthRequest request)
        {
            var response = _tokenInfoRepository.Get(request.username, request.password);
            return JsonConvert.SerializeObject(response);
        }

        [HttpPost]
        public dynamic LogOff([FromBody]AuthResponse token)
        {
            var response = _tokenInfoRepository.LogOff(token);
            return JsonConvert.SerializeObject(response);
        }

        [HttpGet]
        public string GetUserToken(string token, string personID)
        {

            string baseURL = Request.Scheme + "://" + Request.Host.Value;

            var result = _tokenInfoRepository.GetUserToken(token, personID);
            return JsonConvert.SerializeObject(result);

        }
    }
}
