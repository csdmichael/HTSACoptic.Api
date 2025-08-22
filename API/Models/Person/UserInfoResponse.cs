using HTSA.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HTSA.API.Models
{
    public class UserInfoResponse: BasicReponse
    {
        public RegisteredUser user { get; set; }

        public UserInfoResponse()
        {
            user = new RegisteredUser();
        }
    }
}
