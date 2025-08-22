using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HTSA.API.Models
{
    public class RegisteredUser
    {
        public string RegPerson { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string phone { get; set; }
        public string Password { get; set; }
        public string Confirm { get; set; }
        public string DobDay { get; set; }
        public string DobMonth { get; set; }
        public string DobYear { get; set; }
        public string country { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string platform { get; set; }
        public string DOD { get; set; }
        public string Bio { get; set; }
        // public string ActivationCode { get; set; }
    }
}
