using Microsoft.AspNetCore.Mvc;
using HTSA.Repositories;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
//----------------------------------------------------------
//-- Author: Michael Yaacoub
//-- Email: csdmichael@gmail.com
//----------------------------------------------------------

namespace HTSA.Controllers
{
    //[Authorize]
    //[Route("api/[controller]")]
    public class LoginsInfoController : Controller
    {
        ILoginsInfoRepository _LoginsInfoRepository;
        readonly ILogger<LoginsInfoController> _logger;

        public LoginsInfoController(ILogger<LoginsInfoController> logger, ILoginsInfoRepository LoginsInfoRepository)
        {
            _LoginsInfoRepository = LoginsInfoRepository;
            _logger = logger;
        }
        
        //----------------------------------------------------------
        //-- Michael Yaacoub - 1/18/2016
        //-- csdmichael@gmail.com
        //----------------------------------------------------------

        [HttpGet]
        public string GetLoginsByDay(string token)
        {
           
            string baseURL = Request.Scheme + "://" + Request.Host.Value;

            var result = _LoginsInfoRepository.GetLoginsByDay(baseURL, token);
            return JsonConvert.SerializeObject(result);

        }

        [HttpGet]
        public string GetLoginsByModule(string token)
        {

            string baseURL = Request.Scheme + "://" + Request.Host.Value;

            var result = _LoginsInfoRepository.GetLoginsByModule(baseURL, token);
            return JsonConvert.SerializeObject(result);

        }

        [HttpGet]
        public string GetLoginsByUserID(string token)
        {

            string baseURL = Request.Scheme + "://" + Request.Host.Value;

            var result = _LoginsInfoRepository.GetLoginsByUserID(baseURL, token);
            return JsonConvert.SerializeObject(result);

        }

        [HttpGet]
        public string GetLoginDetails(string date, string module, string userID, string token)
        {

            string baseURL = Request.Scheme + "://" + Request.Host.Value;

            var result = _LoginsInfoRepository.GetLoginDetails(date, module, userID, baseURL, token);
            return JsonConvert.SerializeObject(result);

        }
    }
}
