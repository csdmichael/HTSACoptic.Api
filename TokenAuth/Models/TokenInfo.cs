using System;

namespace TokenAuth.Models
{
    public class TokenInfo
    {
        public string Token { get; set; }
        public DateTime? Expires { get; set; }
    }
}
