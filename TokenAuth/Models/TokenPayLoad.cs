using System;
using System.Collections.Generic;
using System.Text;

namespace TokenAuth.Models
{
    public class TokenPayLoad
    {
        public bool IsAuth { get; set; }
        public bool HasPic { get; set; }
        public string PicFileName { get; set; }
        public string PersonId { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Role { get; set; }
        public string DisplayName { get; set; }
        public string Message { get; set; }
        public bool IsAppAdmin { get; set; }
        public string AdminOnChurches { get; set; }

        public TokenPayLoad()
        {
            this.IsAuth = false;
        }
        // public string JWT { get; set; }

    }
}
