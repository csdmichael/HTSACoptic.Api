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
    [Route("api/[controller]")]
    public class AppController : Controller
    {
        IAppRepository _AppRepository;
        readonly ILogger<AppController> _logger;

        public AppController(ILogger<AppController> logger, IAppRepository AppRepository)
        {
            _AppRepository = AppRepository;
            _logger = logger;
        }
        
        //----------------------------------------------------------
        //-- Michael Yaacoub - 10/7/2017
        //-- csdmichael@gmail.com
        //----------------------------------------------------------

        [HttpGet("versions")]
        public string GetAppVersions()
        {
           
            string baseURL = Request.Scheme + "://" + Request.Host.Value;

            var result = _AppRepository.GetAppVersions(baseURL);
            return JsonConvert.SerializeObject(result);
            
        }

        [HttpGet("currentVersion")]
        public string GetCurrentVersion()
        {

            string baseURL = Request.Scheme + "://" + Request.Host.Value;

            var result = _AppRepository.GetCurrentVersion(baseURL);
            return JsonConvert.SerializeObject(result);

        }

    }
}
