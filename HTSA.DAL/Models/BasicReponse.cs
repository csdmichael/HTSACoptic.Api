using System;
using System.Collections.Generic;
using System.Text;

namespace HTSA.DAL.Models
{
    public class BasicReponse
    {
        public BasicReponse()
        {
            IsSuccess = false;
            Message = "";
        }

        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
