using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace HTSA.Models
{
    public class GroupedCalEvents
    {
        private string StartDate;
        public string FormattedStartDate;
        public List<CalEvent> Events;

        public GroupedCalEvents(string startDate)
        {
            this.StartDate = startDate;
            this.FormattedStartDate = GetFormattedDate(startDate);
            this.Events = new List<CalEvent>();
        }

        private string GetFormattedDate(string strDate)
        {
            string formattedDate = "";

            DateTime dt = System.Convert.ToDateTime(strDate);

            formattedDate = string.Format("{0}, {1} {2}, {3}",
                dt.DayOfWeek,
                CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(dt.Month),
                dt.Day,
                dt.Year);

            return formattedDate;
        }
    }
}
