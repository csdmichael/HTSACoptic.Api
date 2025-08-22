using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HTSA.API.Models
{
    public class PushModel
    {
        public string TagNames { get; set; }
        public string TagValues { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Msg { get; set; }
        public bool HasData { get; set; }
        public PushDataModuleAction ModuleAction { get; set; }
    }
}
