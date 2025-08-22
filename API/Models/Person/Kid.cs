//----------------------------------------------------------
//-- Author: Michael Yaacoub
//-- Email: csdmichael@gmail.com
//----------------------------------------------------------
namespace HTSA.Models
{

    public class Kid
    {
        
        public string PersonID { get; set; }
        public string PersonName { get; set; }
        public string DOB { get; set; }
        public string CellPhone { get; set; }
        public string Email { get; set; }
        public string FatherID { get; set; }
        public string FatherName { get; set; }
        public string MotherID { get; set; }
        public string MotherName { get; set; }
        public int AttendPerc { get; set; }
        public int LessonsCount { get; set; }
        public int AttendCount { get; set; }
        public int YearsOld { get; set; }
        public string PictureName { get; set; }
        //public int ClassID { get; set; }
        //public string ClassName { get; set; }
    }
}
