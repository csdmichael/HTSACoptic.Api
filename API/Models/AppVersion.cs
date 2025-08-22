using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HTSA.API.Models
{
    public class AppVersion
    {
        public string VersionId { get; set; }
        public string ReleaseDate { get; set; }
        public string UpgradeAction { get; set; }
        public int IsLatest { get; set; }
        public int FeatureId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
