
//----------------------------------------------------------
//-- Author: Michael Yaacoub
//-- Email: csdmichael@gmail.com
//----------------------------------------------------------
using HTSA.Models;
using System.Collections.Generic;

namespace HTSA.Repositories
{
    public interface ICalendarRepository
    {
        List<CalEvent> GetEvents(string calendarName);
        bool DataRefreshEvents(List<CalEvent> lstCalEvents);

        List<CalEvent> GetEventsFromDB_Today();

        List<CalEvent> GetEventsFromDB_Tomorrow();

        List<CalEvent> GetEventsFromDB_ByDay(string day);

        List<CalEvent> GetEventsFromDB();

        List<GroupedCalEvents> GetGroupedEventsFromDB();
    }
}