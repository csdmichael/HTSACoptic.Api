using Newtonsoft.Json;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.File;

namespace HTSA.Models
{
    public class DynaTreeItem
    {
        public string title { get; set; }
        public bool isFolder { get; set; }
        public string key { get; set; }
        public string relativeFileURL { get; set; }
        public List<DynaTreeItem> children;
        
        public DynaTreeItem()
        {
            children = new List<DynaTreeItem>();
        }

        public string JsonToDynatree()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}