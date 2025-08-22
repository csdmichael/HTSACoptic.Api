

using HTSA.Models;
using System.IO;
using System.Net;
using System.Text;
using HTSA.DAL.Models;
using System;
using System.Collections.Generic;
using HTSA.API.Models;

//----------------------------------------------------------
//-- Author: Michael Yaacoub
//-- Email: csdmichael@gmail.com
//----------------------------------------------------------

namespace HTSA.Repositories
{
    // Create Notification
    // https://documentation.onesignal.com/reference#create-notification

    public class PushRepository : IPushRepository
    {
        
        // ApplContext _context;
        string _apiKey;
        string _appId;

        public PushRepository(
            /*ApplContext context*/
             )
        {
            // _context = context;
            _apiKey = "NGViMmIzZjEtMWQ3ZC00NzI3LWFiYTgtZWE5ZGM5YjBjNzcx";
            _appId = "6a1e7d32-2750-4f0a-bf41-6e4044ea1095";
        }

        private string BuildDataFromModuleAction(PushDataModuleAction moduleAction)
        {
            string data = "{\"ModuleCode\": \"" + moduleAction.ModuleCode + "\", "
                    + "{\"ItemId\": \"" + moduleAction.ItemId + "\", "
                    + "{\"ActionCode\": \"" + moduleAction.ActionCode + "\"}";

            return data;
        }

        public BasicReponse SendBroadcastPush(string title, string subTitle, string msg, bool hasData = false, PushDataModuleAction moduleAction = null)
        {
            return SendTaggedPush("", "", title, subTitle, msg, hasData, moduleAction);
        }

        private string BuildFiltersFromTagNamesValues(string tagNames, string tagValues)
        {
            string filters = "";
            string[] tags;
            string[] vals;

            if (!String.IsNullOrWhiteSpace(tagNames))
            {


                tags = tagNames.Split(',');
                vals = tagValues.Split(',');

                string tagName;
                string tagValue;
                string prefix;

            
                for (int i = 0; i < tags.Length; i++)
                {
                    tagName = tags[i];
                    tagValue = vals[i];
                    if (i == 0)
                        prefix = "";
                    else
                        prefix = ", ";

                    filters += prefix + "{\"field\": \"tag\", \"key\": \"" + tagName + "\", \"relation\": \"=\", \"value\": \"" + tagValue.ToLower() + "\"}";
                }

            }

            return filters;
        }

        public BasicReponse SendTaggedPush(string tagNames, string tagValues, string title, string subTitle, string msg, bool hasData = false, PushDataModuleAction moduleAction = null)
        {
            BasicReponse br = new BasicReponse();

            var request = WebRequest.Create("https://onesignal.com/api/v1/notifications") as HttpWebRequest;

            request.KeepAlive = true;
            request.Method = "POST";
            request.ContentType = "application/json; charset=utf-8";

            request.Headers.Add("authorization", "Basic " + this._apiKey);

            string filterTags = BuildFiltersFromTagNamesValues(tagNames, tagValues);

            string requestJSON = "{"
                                                    + "\"app_id\": \"" + this._appId + "\""
                                                    + ",\"contents\": {\"en\": \"" + msg + "\"}"
                                                    + ",\"headings\": {\"en\": \"" + title + "\"}"
                                                    + (!String.IsNullOrWhiteSpace(subTitle) ? ",\"subtitle\": {\"en\": \"" + subTitle + "\"}" : "")
                                                    + (!String.IsNullOrWhiteSpace(tagNames) ? ",\"filters\": [" + filterTags + "]" : "")
                                                    + (hasData ? ",\"data\": [" + BuildDataFromModuleAction(moduleAction) + "]" : "")
                                                    + ",\"included_segments\": [\"All\"]}";

            byte[] byteArray = Encoding.UTF8.GetBytes(requestJSON);

            string responseContent = null;

            try
            {
                using (var writer = request.GetRequestStream())
                {
                    writer.Write(byteArray, 0, byteArray.Length);
                }

                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        responseContent = reader.ReadToEnd();
                    }
                }

                br.IsSuccess = true;
                br.Message = responseContent;
            }
            catch (WebException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                System.Diagnostics.Debug.WriteLine(new StreamReader(ex.Response.GetResponseStream()).ReadToEnd());


                br.IsSuccess = false;
                br.Message = ex.Message;
            }

            System.Diagnostics.Debug.WriteLine(responseContent);

            return br;
        }
    }
}
