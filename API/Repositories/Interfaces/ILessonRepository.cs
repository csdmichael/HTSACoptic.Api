using HTSA.Models;
//----------------------------------------------------------
//-- Author: Michael Yaacoub
//-- Email: csdmichael@gmail.com
//----------------------------------------------------------

namespace HTSA.Repositories
{
    public interface ILessonRepository
    {
        
        LessonsResponse GetLessonsByClass(string baseURL, string token, string classID, string IsThisWeek);
        LessonsResponse CreateNewLesson(string baseURL, string token, string lessonName, string lessonDescr, string classID, string nextSunday);
        UpdateResponse UpdateLesson(string baseURL, string token, string lessonID, string lessonName, string lessonDescr, string nextSunday);
        PersonResponse UpdateAttendance(string baseURL, string token, string lessonID, string personIDs, string isAttendList);
        PersonResponse GetAttendance(string baseURL, string token, string lessonID);
        LessonsResponse DeleteLesson(string baseURL, string token, string lessonID);

    }
}