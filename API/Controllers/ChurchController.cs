using Microsoft.AspNetCore.Mvc;
using HTSA.Repositories;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using HTSA.Models;
using Microsoft.AspNetCore.Authorization;
using TokenAuth.Models;
using TokenAuth.Repositories;
using System.Threading.Tasks;
//----------------------------------------------------------
//-- Author: Michael Yaacoub
//-- Email: csdmichael@gmail.com
//----------------------------------------------------------

namespace HTSA.Controllers
{
    [Route("api/[controller]")]
    public class ChurchController : Controller
    {
        IChurchRepository _ChurchRepository;
        readonly ILogger<ChurchController> _logger;
        IAuthRepository _authRepository;

        public ChurchController(ILogger<ChurchController> logger, IChurchRepository ChurchRepository, IAuthRepository authRepository)
        {
            _ChurchRepository = ChurchRepository;
            _logger = logger;
            _authRepository = authRepository;
        }
        
        //----------------------------------------------------------
        //-- Michael Yaacoub - 10/7/2017
        //-- csdmichael@gmail.com
        //----------------------------------------------------------

        [HttpGet("list")]
        public string GetChurches(string token, string churchID)
        {
           
            string baseURL = Request.Scheme + "://" + Request.Host.Value;

            var result = _ChurchRepository.GetChurches(baseURL, token, churchID);
            return JsonConvert.SerializeObject(result);
            
        }

        [Authorize]
        [HttpGet("member")]
        public dynamic GetPersonChurches()
        {

            string baseURL = Request.Scheme + "://" + Request.Host.Value;

            TokenPayLoad tokenPayload;
            tokenPayload = _authRepository.DecodeToken(this.Request.Headers["Authorization"]);

            if (tokenPayload.Role == "InactiveUser")
                return Unauthorized("Only Active users can call this method!");

            var result = _ChurchRepository.GetPersonChurches(baseURL, tokenPayload.PersonId);
            return JsonConvert.SerializeObject(result);

        }

        [Authorize]
        [HttpPost("memberadd")]
        public dynamic AddPersonChurch([FromBody] PersonChurch pc)
        {
            TokenPayLoad tokenPayload;
            tokenPayload = _authRepository.DecodeToken(this.Request.Headers["Authorization"]);

            if (tokenPayload.Role == "InactiveUser")
                return Unauthorized("Only Active users can call this method!");
            
            pc.StatusID = "PEND";
            pc.PersonID = tokenPayload.PersonId;

            var response = _ChurchRepository.AddPersonChurch(pc);
            return JsonConvert.SerializeObject(response);
        }

        [Authorize]
        [HttpPost("memberupdate")]
        public dynamic UpdatePersonChurch([FromBody]PersonChurch pc)
        {
            var response = _ChurchRepository.UpdatePersonChurch(pc);
            return JsonConvert.SerializeObject(response);
        }

        [Authorize()]
        [HttpGet("membersrequests/{churchId}")]
        public async Task<IActionResult> GetMembersRequests(string churchId)
        {
            string baseURL = Request.Scheme + "://" + Request.Host.Value;

            TokenPayLoad tokenPayload;
            tokenPayload = _authRepository.DecodeToken(this.Request.Headers["Authorization"]);

            if (!(tokenPayload.IsAppAdmin || (tokenPayload.AdminOnChurches != null && tokenPayload.AdminOnChurches.ToLower().Contains(churchId.ToLower()))))
                return Unauthorized("Only Admins can call this method!");

            var result = await _ChurchRepository.GetMembers(churchId, "PEND", baseURL);
            return Ok(JsonConvert.SerializeObject(result));
        }

        [Authorize()]
        [HttpGet("memberslist/{churchId}")]
        public async Task<IActionResult> GetMembersList(string churchId)
        {
            string baseURL = Request.Scheme + "://" + Request.Host.Value;

            TokenPayLoad tokenPayload;
            tokenPayload = _authRepository.DecodeToken(this.Request.Headers["Authorization"]);

            if (!(tokenPayload.IsAppAdmin || (tokenPayload.AdminOnChurches != null && tokenPayload.AdminOnChurches.ToLower().Contains(churchId.ToLower()))))
                return Unauthorized("Only Admins can call this method!");

            var result = await _ChurchRepository.GetMembers(churchId, "ACC", baseURL);
            return Ok(JsonConvert.SerializeObject(result));
        }

        [Authorize()]
        [HttpPost("member/approve")]
        public async Task<IActionResult> ApproveMember([FromBody] PersonChurch pc)
        {
            string baseURL = Request.Scheme + "://" + Request.Host.Value;

            TokenPayLoad tokenPayload;
            tokenPayload = _authRepository.DecodeToken(this.Request.Headers["Authorization"]);

            if (!(tokenPayload.IsAppAdmin || (tokenPayload.AdminOnChurches != null && tokenPayload.AdminOnChurches.ToLower().Contains(pc.ChurchID.ToLower()))))
                return Unauthorized("Only Admins can call this method!");

            pc.StatusID = "ACC";

            var result = _ChurchRepository.UpdatePersonChurch(pc);
            return Ok(JsonConvert.SerializeObject(result));
        }

        [Authorize()]
        [HttpPost("member/reject")]
        public async Task<IActionResult> RejectMember([FromBody] PersonChurch pc)
        {
            string baseURL = Request.Scheme + "://" + Request.Host.Value;

            TokenPayLoad tokenPayload;
            tokenPayload = _authRepository.DecodeToken(this.Request.Headers["Authorization"]);

            if (!(tokenPayload.IsAppAdmin || (tokenPayload.AdminOnChurches != null && tokenPayload.AdminOnChurches.ToLower().Contains(pc.ChurchID.ToLower()))))
                return Unauthorized("Only Admins can call this method!");

            pc.StatusID = "REJ";

            var result = _ChurchRepository.UpdatePersonChurch(pc);
            return Ok(JsonConvert.SerializeObject(result));
        }
    }
}
