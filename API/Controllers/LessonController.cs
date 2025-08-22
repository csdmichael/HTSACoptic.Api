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
    public class LessonController : Controller
    {
        ILessonRepository _LessonRepository;
        readonly ILogger<LessonController> _logger;

        public LessonController(ILogger<LessonController> logger, ILessonRepository LessonRepository)
        {
            _LessonRepository = LessonRepository;
            _logger = logger;
        }
        
        //----------------------------------------------------------
        //-- Michael Yaacoub - 10/7/2017
        //-- csdmichael@gmail.com
        //----------------------------------------------------------

        [HttpGet]
        public string GetLessonsByClass(string token, string classID, string IsThisWeek)
        {
           
            string baseURL = Request.Scheme + "://" + Request.Host.Value;

            var result = _LessonRepository.GetLessonsByClass(baseURL, token, classID, IsThisWeek);
            return JsonConvert.SerializeObject(result);
            
        }

        [HttpPost]
        public string DeleteLesson([FromBody] Lesson lesson)
        {

            string baseURL = Request.Scheme + "://" + Request.Host.Value;

            var result = _LessonRepository.DeleteLesson(baseURL, lesson.Token, lesson.LessonID.ToString());
            return JsonConvert.SerializeObject(result);

        }

        [HttpPost]
        public string CreateNewLesson([FromBody] Lesson lesson)
        {

            string baseURL = Request.Scheme + "://" + Request.Host.Value;

            var result = _LessonRepository.CreateNewLesson(baseURL, lesson.Token, lesson.LessonName, lesson.LessonDescr, lesson.ClassID.ToString(), lesson.Date);
            return JsonConvert.SerializeObject(result);

        }

        [HttpPost]
        public string UpdateLesson([FromBody] Lesson lesson)
        {

            string baseURL = Request.Scheme + "://" + Request.Host.Value;

            var result = _LessonRepository.UpdateLesson(baseURL, lesson.Token, lesson.LessonID.ToString(), lesson.LessonName, lesson.LessonDescr, lesson.Date);
            return JsonConvert.SerializeObject(result);

        }

        [HttpPost]
        public string UpdateAttendance([FromBody] UpdateAttend ua)
        {
            string baseURL = Request.Scheme + "://" + Request.Host.Value;

            var result = _LessonRepository.UpdateAttendance(baseURL, ua.Token, ua.lessonID, ua.personIDs, ua.isAttendList);
            return JsonConvert.SerializeObject(result);
        }

        [HttpGet]
        public string GetAttendance(string token, string lessonID)
        {
            string baseURL = Request.Scheme + "://" + Request.Host.Value;

            var result = _LessonRepository.GetAttendance(baseURL, token, lessonID);
            return JsonConvert.SerializeObject(result);
        }

    }
}
