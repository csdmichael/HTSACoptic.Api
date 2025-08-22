using System.Collections.Generic;
//----------------------------------------------------------
//-- Author: Michael Yaacoub
//-- Email: csdmichael@gmail.com
//----------------------------------------------------------
namespace HTSA.Models
{
    public class LessonsResponse
    {
        public string parentURL { get; set; }
        public int LessonsListCount { get; set; }
        public List<Lesson> LessonsList { get; set; }

        

    }


}
