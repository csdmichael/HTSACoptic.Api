using System.Collections.Generic;
//----------------------------------------------------------
//-- Author: Michael Yaacoub
//-- Email: csdmichael@gmail.com
//----------------------------------------------------------
namespace HTSA.Models
{
    public class EventLogResponse
    {
        public string parentURL { get; set; }
        public int EventLogsListCount { get; set; }
        public List<EventLogInfo> EventLogsList { get; set; }
        
        

    }


}
