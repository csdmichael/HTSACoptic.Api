
//----------------------------------------------------------
//-- Author: Michael Yaacoub
//-- Email: csdmichael@gmail.com
//----------------------------------------------------------
using HTSA.API.Models;
using HTSA.DAL.Models;

namespace HTSA.Repositories
{
    public interface IPushRepository
    {
        BasicReponse SendBroadcastPush(string title, string subTitle, string msg, bool hasData = false, PushDataModuleAction moduleAction = null);
        BasicReponse SendTaggedPush(string tagName, string tagValue, string title, string subTitle, string msg, bool hasData = false, PushDataModuleAction moduleAction = null);
        // string BuildDataFromModuleAction(PushDataModuleAction moduleAction);
        // string BuildFiltersFromTagNamesValues(string tagNames, string tagValues);
    }
}