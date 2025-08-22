using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HTSA.API.Models
{
    public class User
    {
        public string ACCESS_TOKEN { get; set; }
        public DateTime EXPIRES_IN { get; set; }
    }
}
