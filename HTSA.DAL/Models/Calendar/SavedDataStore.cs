using Google.Apis.Util.Store;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
//----------------------------------------------------------
//-- Author: Michael Yaacoub
//-- Email: csdmichael@gmail.com
//----------------------------------------------------------
namespace HTSA.Models
{

    /// 
    /// Saved data store that implements . 
    /// This Saved data store stores a StoredResponse object.
    /// 
    public class SavedDataStore : IDataStore
    {
        public StoredResponse _storedResponse { get; set; }
        /// 
        /// Constructs Load previously saved StoredResponse.
        /// 
        ///Stored response
        public SavedDataStore(StoredResponse pResponse)
        {
            this._storedResponse = pResponse;
        }
        public SavedDataStore()
        {
            this._storedResponse = new StoredResponse();
        }
        /// 
        /// Stores the given value. into storedResponse
        /// .
        /// 
        ///The type to store in the data store
        ///The key
        ///The value to store in the data store
        public Task StoreAsync<T>(string key, T value)
        {
            var serialized = Google.Apis.Json.NewtonsoftJsonSerializer.Instance.Serialize(value);
            JObject jObject = JObject.Parse(serialized);
            // storing access token
            var test = jObject.SelectToken("access_token");
            if (test != null)
            {
                this._storedResponse.access_token = (string)test;
            }
            // storing token type
            test = jObject.SelectToken("token_type");
            if (test != null)
            {
                this._storedResponse.token_type = (string)test;
            }
            test = jObject.SelectToken("expires_in");
            if (test != null)
            {
                this._storedResponse.expires_in = (long?)test;
            }
            test = jObject.SelectToken("refresh_token");
            if (test != null)
            {
                this._storedResponse.refresh_token = (string)test;
            }
            test = jObject.SelectToken("Issued");
            if (test != null)
            {
                this._storedResponse.Issued = (string)test;
            }
            return Task.Delay(0);
        }

        /// 
        /// Deletes StoredResponse.
        /// 
        ///The key to delete from the data store
        public Task DeleteAsync<T>(string key)
        {
            this._storedResponse = new StoredResponse();
            return Task.Delay(0);
        }

        /// 
        /// Returns the stored value for_storedResponse      
        ///The type to retrieve
        ///The key to retrieve from the data store
        /// The stored object
        public Task<T> GetAsync<T>(string key)
        {
            TaskCompletionSource<T> tcs = new TaskCompletionSource<T>();
            try
            {
                string JsonData = Newtonsoft.Json.JsonConvert.SerializeObject(this._storedResponse);
                tcs.SetResult(Google.Apis.Json.NewtonsoftJsonSerializer.Instance.Deserialize<T>(JsonData));
            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
            }
            return tcs.Task;
        }

        /// 
        /// Clears all values in the data store. 
        /// 
        public Task ClearAsync()
        {
            this._storedResponse = new StoredResponse();
            return Task.Delay(0);
        }

        ///// Creates a unique stored key based on the key and the class type.
        /////The object key
        /////The type to store or retrieve
        //public static string GenerateStoredKey(string key, Type t)
        //{
        //    return string.Format("{0}-{1}", t.FullName, key);
        //}
    }
}
