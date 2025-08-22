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
    public class PrivacyController : Controller
    {
        IPrivacyRepository _PrivacyRepository;
        readonly ILogger<PrivacyController> _logger;

        public PrivacyController(ILogger<PrivacyController> logger, IPrivacyRepository PrivacyRepository)
        {
            _PrivacyRepository = PrivacyRepository;
            _logger = logger;
        }
        
        //----------------------------------------------------------
        //-- Michael Yaacoub - 10/7/2017
        //-- csdmichael@gmail.com
        //----------------------------------------------------------

        [HttpGet]
        public string GetLookups(string token)
        {
           
            string baseURL = Request.Scheme + "://" + Request.Host.Value;

            var result = _PrivacyRepository.GetLookups(baseURL, token);
            return JsonConvert.SerializeObject(result);
            
        }

        [HttpGet]
        public string GetPersonPrivSettings(string token, string PersonID, string PrivacySettingID)
        {

            string baseURL = Request.Scheme + "://" + Request.Host.Value;

            var result = _PrivacyRepository.GetPersonPrivSettings(baseURL, token, PersonID, PrivacySettingID);
            return JsonConvert.SerializeObject(result);

        }

        [HttpGet]
        public string GetPersonChurchRole(string token, string ChurchID, string ChurchRoleID, string PersonID)
        {

            string baseURL = Request.Scheme + "://" + Request.Host.Value;

            var result = _PrivacyRepository.GetPersonChurchRole(baseURL, token, ChurchID, ChurchRoleID, PersonID);
            return JsonConvert.SerializeObject(result);

        }

        [HttpGet]
        public string GetPersonModuleRole(string token, string ChurchID, string ModuleRoleID, string ModuleID, string PersonID)
        {

            string baseURL = Request.Scheme + "://" + Request.Host.Value;

            var result = _PrivacyRepository.GetPersonModuleRole(baseURL, token, ChurchID, ModuleRoleID, ModuleID, PersonID);
            return JsonConvert.SerializeObject(result);

        }

        [HttpPost]
        public dynamic SetPersonPrivSettings([FromBody]PersonPrivSettingsUpdateResponse ppsr)
        {
            var response = _PrivacyRepository.SetPersonPrivSettings(ppsr);
            return JsonConvert.SerializeObject(response);
        }

    }
}
