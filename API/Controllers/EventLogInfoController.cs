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
    public class EventLogInfoController : Controller
    {
        IEventLogInfoRepository _EventLogInfoRepository;
        readonly ILogger<EventLogInfoController> _logger;

        public EventLogInfoController(ILogger<EventLogInfoController> logger, IEventLogInfoRepository EventLogInfoRepository)
        {
            _EventLogInfoRepository = EventLogInfoRepository;
            _logger = logger;
        }
        
        //----------------------------------------------------------
        //-- Michael Yaacoub - 12/21/2016
        //-- csdmichael@gmail.com
        //----------------------------------------------------------

        [HttpGet]
        public string GetEventLogsInfo(string eventID, string module, string type, string messageKeyword, string startDate, string endDate, string token)
        {
            if (eventID == null)
                eventID = "";
            if (startDate == null)
                startDate = "";
            if (endDate == null)
                endDate = "";
            if (messageKeyword == null)
                messageKeyword = "";
            if (type == null)
                type = "";
            if (module == null)
                module = "";

            string baseURL = Request.Scheme + "://" + Request.Host.Value;

            var result = _EventLogInfoRepository.GetEventLogsInfo(eventID, module, type, messageKeyword, startDate, endDate, baseURL, token);
            return JsonConvert.SerializeObject(result);

        }

        [HttpGet]
        public string GetRowCountsInfo(string eventID, string messageKeyword, string startDate, string endDate, string token)
        {
            if (eventID == null)
                eventID = "";
            if (startDate == null)
                startDate = "";
            if (endDate == null)
                endDate = "";
            if (messageKeyword == null)
                messageKeyword = "";

            string baseURL = Request.Scheme + "://" + Request.Host.Value;

            var result = _EventLogInfoRepository.GetRowCountsInfo(eventID, messageKeyword, startDate, endDate, baseURL, token);
            return JsonConvert.SerializeObject(result);

        }
    }
}
