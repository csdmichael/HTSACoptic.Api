using Microsoft.AspNetCore.Mvc;
using HTSA.Repositories;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using HTSA.Models;
using System.Threading.Tasks;
using HTSA.API.Models;
using TokenAuth.Models;
using System;
using TokenAuth.Repositories;
using Microsoft.AspNetCore.Authorization;
//----------------------------------------------------------
//-- Author: Michael Yaacoub
//-- Email: csdmichael@gmail.com
//----------------------------------------------------------

namespace HTSA.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    public class PersonController : Controller
    {
        
        IPersonRepository _PersonRepository;
        readonly ILogger<PersonController> _logger;
        IAuthRepository _authRepository;

        public PersonController(ILogger<PersonController> logger, IPersonRepository personRepository, IAuthRepository authRepository)
        {
            _PersonRepository = personRepository;
            _logger = logger;
            _authRepository = authRepository;
        }

        #region No Authorize Methods 

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisteredUser registeredUser)
        {
            string baseURL = Request.Scheme + "://" + Request.Host.Value;

            var result = await _PersonRepository.RegisterUser(registeredUser, baseURL);
            return Ok(JsonConvert.SerializeObject(result));
        }

        

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] AuthRequest authRequest)
        {
            string baseURL = Request.Scheme + "://" + Request.Host.Value;

            var result = await _PersonRepository.LoginUser(authRequest, baseURL);
            return Ok(JsonConvert.SerializeObject(result));
        }

        [HttpPost("passwordreset")]
        public async Task<IActionResult> ResetPassword([FromBody] EmailObj emailObj)
        {
            string baseURL = Request.Scheme + "://" + Request.Host.Value;

            var result = await _PersonRepository.ResetPassword(emailObj.Email, baseURL);
            return Ok(JsonConvert.SerializeObject(result));
        }

        #endregion

        #region Authorize Methods

        [Authorize()]
        [HttpPost("update")]
        public async Task<IActionResult> UpdateUser([FromBody] RegisteredUser regUser)
        {
            string baseURL = Request.Scheme + "://" + Request.Host.Value;
            TokenPayLoad tokenPayload;
            tokenPayload = _authRepository.DecodeToken(this.Request.Headers["Authorization"]);

            if (tokenPayload.Role == "InactiveUser")
                return Unauthorized("Only Active users can call this method!");

            var result = _PersonRepository.UpdateUser(tokenPayload.PersonId, regUser, baseURL);
            return Ok(JsonConvert.SerializeObject(result));
        }


        [Authorize()]
        [HttpGet("userdetails")]
        public async Task<IActionResult> GetUserDetails()
        {
            string baseURL = Request.Scheme + "://" + Request.Host.Value;

            TokenPayLoad tokenPayload;
            tokenPayload = _authRepository.DecodeToken(this.Request.Headers["Authorization"]);

            if (tokenPayload.Role == "InactiveUser")
                return Unauthorized("Only Active users can call this method!");

            var result = await _PersonRepository.GetUserDetails(tokenPayload.PersonId, baseURL);
            return Ok(JsonConvert.SerializeObject(result));
        }

        [Authorize()]
        [HttpGet("userinfo")]
        public async Task<IActionResult> GetUserInfo()
        {
            string baseURL = Request.Scheme + "://" + Request.Host.Value;

            TokenPayLoad tokenPayload;
            tokenPayload = _authRepository.DecodeToken(this.Request.Headers["Authorization"]);

            if (tokenPayload.Role == "InactiveUser")
                return Unauthorized("Only Active users can call this method!");
            
            return Ok(JsonConvert.SerializeObject(tokenPayload));
        }

        [Authorize()]
        [HttpGet("people")]
        public async Task<IActionResult> GetPeople(int isActive, string searchKeyword, int isSaint = 0)
        {
            string baseURL = Request.Scheme + "://" + Request.Host.Value;

            TokenPayLoad tokenPayload;
            tokenPayload = _authRepository.DecodeToken(this.Request.Headers["Authorization"]);

            if (tokenPayload.Role == "InactiveUser")
                return Unauthorized("Only Active users can call this method!");

            var result = await _PersonRepository.GetPeople(tokenPayload.PersonId, isActive, isSaint, searchKeyword, baseURL);
            return Ok(JsonConvert.SerializeObject(result));
        }

        [HttpGet("saints")]
        public async Task<IActionResult> GetSaints(int isActive, string searchKeyword)
        {
            string baseURL = Request.Scheme + "://" + Request.Host.Value;
            
            var result = await _PersonRepository.GetPeople("", isActive, 1, searchKeyword, baseURL);
            return Ok(JsonConvert.SerializeObject(result));
        }

        [Authorize()]
        [HttpGet("details/{personId}")]
        public async Task<IActionResult> GetPersonDetails(string personId)
        {
            string baseURL = Request.Scheme + "://" + Request.Host.Value;

            TokenPayLoad tokenPayload;
            tokenPayload = _authRepository.DecodeToken(this.Request.Headers["Authorization"]);

            if (tokenPayload.Role == "InactiveUser")
                return Unauthorized("Only Active users can call this method!");

            var result = await _PersonRepository.GetUserDetails(personId, baseURL);
            return Ok(JsonConvert.SerializeObject(result));
        }

        [HttpGet("saintdetails/{personId}")]
        public async Task<IActionResult> GetSaintDetails(string personId)
        {
            string baseURL = Request.Scheme + "://" + Request.Host.Value;

            UserInfoResponse result = await _PersonRepository.GetUserDetails(personId, baseURL);
            if (!String.IsNullOrWhiteSpace(result.user.DOD))
                return Ok(JsonConvert.SerializeObject(result));
            else
                return Unauthorized();
        }

        [Authorize()]
        [HttpPost("phoneverify")]
        public async Task<IActionResult> VerifyPhone([FromBody] SMSObj smsObj)
        {
            string baseURL = Request.Scheme + "://" + Request.Host.Value;
            TokenPayLoad tokenPayload;
            tokenPayload = _authRepository.DecodeToken(this.Request.Headers["Authorization"]);

            if (tokenPayload.Role == "InactiveUser")
                return Unauthorized("Only Active users can call this method!");

            var result = await _PersonRepository.VerifyPhone(smsObj.Phone, baseURL);
            return Ok(JsonConvert.SerializeObject(result));
        }

        [Authorize()]
        // [Authorize(Roles = "InactiveUser")]
        [HttpPost("accountactivationcode")]
        public async Task<IActionResult> SendAccountActivationCode()
        {
            string baseURL = Request.Scheme + "://" + Request.Host.Value;
            TokenPayLoad tokenPayload;
            tokenPayload = _authRepository.DecodeToken(this.Request.Headers["Authorization"]);

            if (tokenPayload.Role != "InactiveUser")
                return Unauthorized("Only Inactive users can request new activation code!");

            var result = await _PersonRepository.SendAccountActivationCode(tokenPayload.Email, baseURL);
            return Ok(JsonConvert.SerializeObject(result));
        }

        [Authorize()]
        //[Authorize(Roles = "InactiveUser")]
        [HttpPost("accountactivate")]
        public async Task<IActionResult> ActivateAccount([FromBody] StrObj strObj)
        {
            string baseURL = Request.Scheme + "://" + Request.Host.Value;
            TokenPayLoad tokenPayload;
            tokenPayload = _authRepository.DecodeToken(this.Request.Headers["Authorization"]);

            if (tokenPayload.Role != "InactiveUser")
                return Unauthorized("Only Inactive users can activate their account!");

            var result = await _PersonRepository.ActivateAccount(tokenPayload.Email, strObj.StrValue, baseURL);
            return Ok(JsonConvert.SerializeObject(result));
        }

        [Authorize()]
        //[Authorize(Roles = "InactiveUser")]
        [HttpPost("accountdelete")]
        public async Task<IActionResult> DeleteAccount()
        {
            string baseURL = Request.Scheme + "://" + Request.Host.Value;
            TokenPayLoad tokenPayload;
            tokenPayload = _authRepository.DecodeToken(this.Request.Headers["Authorization"]);

            if (tokenPayload.Role != "InactiveUser")
                return Unauthorized("Only Inactive users can delete their account!");

            var result = await _PersonRepository.DeleteAccount(tokenPayload.Email, baseURL);
            return Ok(JsonConvert.SerializeObject(result));
        }

        [Authorize()]
        //[Authorize(Roles = "InactiveUser")]
        [HttpPost("passwordchange")]
        public async Task<IActionResult> ChangePassword([FromBody] PersonPasswordChange pwc)
        {
            string baseURL = Request.Scheme + "://" + Request.Host.Value;
            TokenPayLoad tokenPayload;
            tokenPayload = _authRepository.DecodeToken(this.Request.Headers["Authorization"]);

            if (tokenPayload.Role == "InactiveUser")
                return Unauthorized("Only Active users can call this method!");

            var result = await _PersonRepository.ChangePassword(pwc, tokenPayload.PersonId, baseURL);
            return Ok(JsonConvert.SerializeObject(result));
        }

        #endregion

        #region Old Methods
        /*
        [Authorize()]
        [HttpPost("Login")]
        public AuthResponse Login([FromBody] AuthRequest request)
        {

            try
            {
                AuthResponse authResp = null;

                if (!string.IsNullOrEmpty(request.UserId) &&
                    !string.IsNullOrEmpty(request.Password))
                {
                    authResp = _authRepository.GetToken(request.UserId, request.Password);

                    return authResp;
                }

                return new AuthResponse
                {
                    IsAuth = false,
                    Message = (authResp != null && !string.IsNullOrEmpty(authResp.Message)) ?
                        authResp.Message : "User couldn't login, empty userId or password"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UserController/Login() encountered an error");

                return new AuthResponse
                {
                    IsAuth = false,
                    Message = "Login Error. Please Try Again Later."
                };
            }
        }
        */


        //----------------------------------------------------------
        //-- Michael Yaacoub - 10/7/2017
        //-- csdmichael@gmail.com
        //----------------------------------------------------------
        /*
        [HttpGet]
        public string GetPerson(string token, string personID, string churchID, string MyFriends, string ChurchRoleID, string RequestorID,
            string isAddFriend, string isAddPriest, string isAddDeacon, string isAddChurchMember, string AddChurchID, string isAddClassMember, string AddClassID, string ClassRole)
        {

            string baseURL = Request.Scheme + "://" + Request.Host.Value;

            var result = _PersonRepository.GetPerson(baseURL, token, personID, churchID, MyFriends, ChurchRoleID, RequestorID,
                            isAddFriend, isAddPriest, isAddDeacon, isAddChurchMember, AddChurchID, isAddClassMember, AddClassID, ClassRole);
            return JsonConvert.SerializeObject(result);
            
        }

        [HttpGet]
        public string GetBirthdaysOfWeek(string token, string churchID, string classID, string nextSunday, string MonthOrWeek, string Month)
        {

            string baseURL = Request.Scheme + "://" + Request.Host.Value;

            var result = _PersonRepository.GetBirthdaysOfWeek(baseURL, token, churchID, classID, nextSunday, MonthOrWeek, Month);
            return JsonConvert.SerializeObject(result);

        }
		
		[HttpGet]
        public string GetLookups(string token)
        {

            string baseURL = Request.Scheme + "://" + Request.Host.Value;

            var result = _PersonRepository.GetLookups(baseURL, token);
            return JsonConvert.SerializeObject(result);

        }

       
        [HttpPost]
        public dynamic ChangePassword([FromBody]PersonPasswordChange pc)
        {
            var response = _PersonRepository.ChangePassword(pc);
            return JsonConvert.SerializeObject(response);
        }

        [HttpPost]
        public dynamic AddPersonRelation([FromBody]PersonRelation pr)
        {
            var response = _PersonRepository.UpdatePersonRelation(pr);
            return JsonConvert.SerializeObject(response);
        }

        [HttpPost]
        public dynamic UpdatePersonRelation([FromBody]PersonRelation pr)
        {
            var response = _PersonRepository.UpdatePersonRelation(pr);
            return JsonConvert.SerializeObject(response);
        }

        [HttpPost]
        public dynamic UpdatePerson([FromBody]Person pr)
        {
            string baseURL = Request.Scheme + "://" + Request.Host.Value;
            var response = _PersonRepository.UpdatePerson(baseURL, pr);
            return JsonConvert.SerializeObject(response);
        }

        
        [HttpGet]
        public string GetPersonRelations(string token, string PersonID, string Relation)
        {

            string baseURL = Request.Scheme + "://" + Request.Host.Value;

            var result = _PersonRepository.GetPersonRelations(baseURL, token, PersonID, Relation);
            return JsonConvert.SerializeObject(result);

        }

        [HttpGet]
        public string GetRelationLookup(string token, string personID)
        {

            string baseURL = Request.Scheme + "://" + Request.Host.Value;

            var result = _PersonRepository.GetRelationLookup(baseURL, token);
            return JsonConvert.SerializeObject(result);

        }

        */
        #endregion
    }
}
