//----------------------------------------------------------
//-- Author: Michael Yaacoub
//-- Email: csdmichael@gmail.com
//----------------------------------------------------------

namespace HTSA.Models
{
    public class LoginsByDayInfo
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public int LoginsCount { get; set; }
        public int SuccessLoginsCount { get; set; }
        public string DateValue { get; set; }

    }


}
