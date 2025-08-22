//----------------------------------------------------------
//-- Author: Michael Yaacoub
//-- Email: csdmichael@gmail.com
//----------------------------------------------------------
namespace HTSA.Models
{

    public class UpdateAttend
    {
        
        public string Token { get; set; }
        public string lessonID { get; set; }
        public string personIDs { get; set; }
        public string isAttendList { get; set; }
    }
}
