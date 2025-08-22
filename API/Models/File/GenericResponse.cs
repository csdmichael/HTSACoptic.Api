using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HTSA.Models
{
    public class GenericResponse
    {
        public bool success { get; set; }
        public string value { get; set; }
        public string messgae { get; set; }
        public string fileName { get; set; }
        public DynaTreeItem data { get; set; }
    }
}