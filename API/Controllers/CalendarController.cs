
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
    public class CalendarController : Controller
    {
        ICalendarRepository _CalendarRepository;
        readonly ILogger<PostController> _logger;

        public CalendarController(ILogger<PostController> logger, ICalendarRepository CalendarRepository)
        {
            _CalendarRepository = CalendarRepository;
            _logger = logger;
        }
        
        //----------------------------------------------------------
        //-- Michael Yaacoub - 10/7/2017
        //-- csdmichael@gmail.com
        //----------------------------------------------------------

        [HttpGet]
        public IActionResult GetEvents(string churchID, string calendarName)
        {
           
            string baseURL = Request.Scheme + "://" + Request.Host.Value;

            var result = _CalendarRepository.GetEvents(calendarName);
            return Ok(JsonConvert.SerializeObject(result));
            
        }

        [HttpGet]
        public IActionResult GetEventsFromDB()
        {

            string baseURL = Request.Scheme + "://" + Request.Host.Value;

            var result = _CalendarRepository.GetEventsFromDB();
            return Ok(JsonConvert.SerializeObject(result));

        }

        [HttpGet]
        public IActionResult GetGroupedEventsFromDB()
        {

            string baseURL = Request.Scheme + "://" + Request.Host.Value;

            var result = _CalendarRepository.GetGroupedEventsFromDB();
            return Ok(JsonConvert.SerializeObject(result));

        }

        [HttpGet]
        public IActionResult GetEventsFromDBToday()
        {

            string baseURL = Request.Scheme + "://" + Request.Host.Value;

            var result = _CalendarRepository.GetEventsFromDB_Today();
            return Ok(JsonConvert.SerializeObject(result));

        }

    }
}
