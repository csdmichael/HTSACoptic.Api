using Microsoft.AspNetCore.Mvc;
using HTSA.Repositories;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using HTSA.Models;
//----------------------------------------------------------
//-- Author: Michael Yaacoub
//-- Email: csdmichael@gmail.com
//----------------------------------------------------------

namespace HTSA.Controllers
{
    //[Authorize]
    //[Route("api/[controller]")]
    public class SSClassController : Controller
    {
        ISSClassRepository _SSClassRepository;
        IKidRepository _KidRepository;
        readonly ILogger<SSClassController> _logger;

        public SSClassController(ILogger<SSClassController> logger, ISSClassRepository SSClassRepository, IKidRepository kidRepository)
        {
            _SSClassRepository = SSClassRepository;
            _KidRepository = kidRepository;
            _logger = logger;
        }
        
        //----------------------------------------------------------
        //-- Michael Yaacoub - 1/18/2016
        //-- csdmichael@gmail.com
        //----------------------------------------------------------

        [HttpGet]
        public string GetSSClasses(string token, string churchID, string classID)
        {
           
            string baseURL = Request.Scheme + "://" + Request.Host.Value;

            var result = _SSClassRepository.GetClassesByChurch(baseURL, token, churchID, classID);
            return JsonConvert.SerializeObject(result);
            //return "{\"ClassesListCount\":1,\"ClassesList\":[{\"ChurchID\":\"USCTHMDN\",\"ClassID\":\"USCTHMDN-M-8\",\"ClassName\":\"St. Antonious\",\"Sex\":\"M\",\"StartDate\":\"2017-09-09\",\"FinishDate\":\"2018-09-09\",\"Grade\":\"8\"}]}";

        }

        
        [HttpGet]
        public string GetKidsByClass(string token, string classID)
        {

            string baseURL = Request.Scheme + "://" + Request.Host.Value;

            var result = _KidRepository.GetKidsByClass(baseURL, token, classID);
            return JsonConvert.SerializeObject(result);
            
        }

        [HttpGet]
        public string GetServantsByClass(string token, string classID)
        {

            string baseURL = Request.Scheme + "://" + Request.Host.Value;

            var result = _SSClassRepository.GetServantsByClass(baseURL, token, classID);
            return JsonConvert.SerializeObject(result);

        }

        [HttpPost]
        public string AddClass(string token, SSClass ssClass)
        {

            string baseURL = Request.Scheme + "://" + Request.Host.Value;

            var result = _SSClassRepository.AddClass(baseURL, ssClass);
            return JsonConvert.SerializeObject(result);

        }

        [HttpPost]
        public string DeleteClass([FromBody] SSClass ssClass)
        {

            string baseURL = Request.Scheme + "://" + Request.Host.Value;

            var result = _SSClassRepository.DeleteClass(baseURL, ssClass);
            return JsonConvert.SerializeObject(result);

        }

        [HttpPost]
        public string AddMember([FromBody] ClassPerson cp)
        {

            string baseURL = Request.Scheme + "://" + Request.Host.Value;

            var result = _SSClassRepository.AddMember(baseURL, cp);
            return JsonConvert.SerializeObject(result);

        }

        [HttpPost]
        public string RemoveMember([FromBody] ClassPerson cp)
        {

            string baseURL = Request.Scheme + "://" + Request.Host.Value;

            var result = _SSClassRepository.RemoveMember(baseURL, cp);
            return JsonConvert.SerializeObject(result);

        }
    }
}
