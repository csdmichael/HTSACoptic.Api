using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HTSA.DAL.Models;
using TokenAuth.Models;

namespace HTSA.API.Models
{
    public class RegisterResponse
    {
        public AuthResponse user { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

        public RegisterResponse()
        {
            user = new AuthResponse();
        }
    }
}
