using System;

namespace TokenAuth.Models
{
    public class AuthResponse : TokenPayLoad
    {
        public string ACCESS_TOKEN { get; set; }
        public DateTime EXPIRES_IN { get; set; }
    }
}
