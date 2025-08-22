using HTSA.Models;
//----------------------------------------------------------
//-- Author: Michael Yaacoub
//-- Email: csdmichael@gmail.com
//----------------------------------------------------------

namespace HTSA.Repositories
{
    public interface IEventLogInfoRepository
    {

        EventLogResponse GetEventLogsInfo(string eventID, string module, string type, string messageKeyword, string startDate, string endDate, string baseURL, string token);
        EventLogResponse GetRowCountsInfo(string eventID, string messageKeyword, string startDate, string endDate, string baseURL, string token);
    }
}