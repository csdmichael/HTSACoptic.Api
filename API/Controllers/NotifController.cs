
using Microsoft.AspNetCore.Mvc;
using HTSA.Repositories;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using HTSA.API.Models;
//----------------------------------------------------------
//-- Author: Michael Yaacoub
//-- Email: csdmichael@gmail.com
//----------------------------------------------------------

namespace HTSA.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    public class NotifController : Controller
    {
        INotifRepository _notifRepository;
        IPushRepository _pushRepository;
        readonly ILogger<PostController> _logger;

        public NotifController(ILogger<PostController> logger, INotifRepository notifRepository, IPushRepository pushRepository)
        {
            _notifRepository = notifRepository;
            _pushRepository = pushRepository;
            _logger = logger;
        }
        
        //----------------------------------------------------------
        //-- Michael Yaacoub - 10/7/2017
        //-- csdmichael@gmail.com
        //----------------------------------------------------------

        

        [HttpPost("email")]
        public async Task<IActionResult> SendEmail([FromBody] EmailModel emailObj)
        {

            string baseURL = Request.Scheme + "://" + Request.Host.Value;

            var result = await _notifRepository.SendEmail(emailObj);
            return Ok(JsonConvert.SerializeObject(result));

        }

        [HttpPost("push/broadcast")]
        public async Task<IActionResult> SendPushBroadcast([FromBody] PushModel pushObj)
        {

            string baseURL = Request.Scheme + "://" + Request.Host.Value;

            var result = _pushRepository.SendBroadcastPush(pushObj.Title, pushObj.SubTitle, pushObj.Msg, pushObj.HasData, pushObj.ModuleAction);
            return Ok(JsonConvert.SerializeObject(result));

        }

        [HttpPost("push/tagged")]
        public async Task<IActionResult> SendPushTagged([FromBody] PushModel pushObj)
        {

            string baseURL = Request.Scheme + "://" + Request.Host.Value;

            var result = _pushRepository.SendTaggedPush(pushObj.TagNames, pushObj.TagValues,
                pushObj.Title, pushObj.SubTitle, pushObj.Msg, pushObj.HasData, pushObj.ModuleAction);
            return Ok(JsonConvert.SerializeObject(result));

        }

    }
}
