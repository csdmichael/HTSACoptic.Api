using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HTSA.API.Models
{
    public class UploadFileRequest
    {
        public string Base64File { get; set; }
        public string PicFileName { get; set; }
        public string Container { get; set; }
        public string FileExt { get; set; }
    }
}
