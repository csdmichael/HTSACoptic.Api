using Microsoft.AspNetCore.Mvc;
using HTSA.Repositories;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using HTSA.Models;
using System.Threading.Tasks;
using HTSA.API.Models;
using TokenAuth.Models;
using System;
using TokenAuth.Repositories;
using Microsoft.AspNetCore.Authorization;
using HTSA.DAL.Models;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using System.Text;
//----------------------------------------------------------
//-- Author: Michael Yaacoub
//-- Email: csdmichael@gmail.com
//----------------------------------------------------------

namespace HTSA.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    public class FileController : Controller
    {

        IFileRepository _FileRepository;
        IPersonRepository _PersonRepository;
        readonly ILogger<FileController> _logger;
        IAuthRepository _authRepository;

        public FileController(ILogger<FileController> logger, IFileRepository fileRepository, IAuthRepository authRepository, IPersonRepository personRepository)
        {
            _PersonRepository = personRepository;
            _logger = logger;
            _authRepository = authRepository;
            _FileRepository = fileRepository;
        }


        [Authorize()]
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile([FromBody] UploadFileRequest ufr)
        {
            // https://onlinejpgtools.com/convert-jpg-to-base64

            string baseURL = Request.Scheme + "://" + Request.Host.Value;

            TokenPayLoad tokenPayload;
            tokenPayload = _authRepository.DecodeToken(this.Request.Headers["Authorization"]);

            if (tokenPayload.Role == "InactiveUser")
                return Unauthorized("Only Active users can call this method!");

            string fileName = tokenPayload.PersonId;

            //string fileName = "123";


            var br = await _FileRepository.UploadFileToBlobStorage(ufr.Base64File, ufr.Container, fileName, ufr.FileExt);

            return Ok(JsonConvert.SerializeObject(br));
        }

        [Authorize()]
        [HttpPost("profilepicupload")]
        public async Task<IActionResult> UploadProfilePic([FromBody] UploadFileRequest ufr)
        {
            // https://onlinejpgtools.com/convert-jpg-to-base64

            string baseURL = Request.Scheme + "://" + Request.Host.Value;

            TokenPayLoad tokenPayload;
            tokenPayload = _authRepository.DecodeToken(this.Request.Headers["Authorization"]);

            if (tokenPayload.Role == "InactiveUser")
                return Unauthorized("Only Active users can call this method!");

            string fileName = ufr.PicFileName.ToLower() + "." + ufr.FileExt;

            //string fileName = "123";


            var br = await _FileRepository.UploadFileToBlobStorage(ufr.Base64File, ufr.Container, fileName, ufr.FileExt);
            AuthResponse ar = new AuthResponse();

            if (br.IsSuccess)
            {
                ar = await _PersonRepository.SetHasPic(1, tokenPayload.PersonId, baseURL, tokenPayload, fileName);
                // System.Threading.Thread.Sleep(10000);
            }

            return Ok(JsonConvert.SerializeObject(ar));
        }

        [Authorize()]
        [HttpPost("postpicupload")]
        public async Task<IActionResult> UploadPostPic([FromBody] UploadFileRequest ufr)
        {
            // https://onlinejpgtools.com/convert-jpg-to-base64

            string baseURL = Request.Scheme + "://" + Request.Host.Value;

            TokenPayLoad tokenPayload;
            tokenPayload = _authRepository.DecodeToken(this.Request.Headers["Authorization"]);

            if (tokenPayload.Role == "InactiveUser")
                return Unauthorized("Only Active users can call this method!");

            string fileName = ufr.PicFileName + "." + ufr.FileExt;

            //string fileName = "123";


            var br = await _FileRepository.UploadFileToBlobStorage(ufr.Base64File, ufr.Container, fileName, ufr.FileExt);

            return Ok(JsonConvert.SerializeObject(br));
        }

        /*
        [Authorize()]
        [HttpPost("profilepicuploadweb")]
        public async Task<IActionResult> UploadProfilePicWeb()
        {
            
            string baseURL = Request.Scheme + "://" + Request.Host.Value;

            TokenPayLoad tokenPayload;
            tokenPayload = _authRepository.DecodeToken(this.Request.Headers["Authorization"]);

            if (tokenPayload.Role == "InactiveUser")
                return Unauthorized("Only Active users can call this method!");

            string fileName = tokenPayload.PersonId;


            var file = Request.Form.Files[0];

            // full path to file in temp location
            var filePath = Path.GetTempFileName();

            var stream = new FileStream(filePath, FileMode.Create);
            var br = new BasicReponse();

            if (file.Length > 0)
            {
                await file.CopyToAsync(stream);
                Byte[] fileBytes = ReadFully(stream);
                br = await _FileRepository.UploadToBlobStorage(fileBytes, "people", fileName, "jpg");
            } 
            else
            {
                br.IsSuccess = false;
            }

            if (br.IsSuccess)
            {
                br = await _PersonRepository.SetHasPic(1, tokenPayload.PersonId, baseURL);
            }

            return Ok(JsonConvert.SerializeObject(br));
        }
       

        public byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
         */


        // [Authorize()]
        [HttpGet("library")]
        public async Task<IActionResult> ReadLibrary(string rootFolder)
        {
            // https://onlinejpgtools.com/convert-jpg-to-base64

            string baseURL = Request.Scheme + "://" + Request.Host.Value;
            GenericResponse gr = new GenericResponse();
            gr.value += "[1] Started. ";

            try
            {

                gr.value += "[2] Tokens match. ";

                // DEFINE THE PATH WHERE WE WANT TO SAVE THE FILES.

                DynaTreeItem di = await _FileRepository.BuildDynaTree(rootFolder);
                string result = di.JsonToDynatree();

                gr.success = true;
                gr.data = di;
                gr.messgae = "Success";


            }
            catch (Exception ex)
            {
                gr.success = false;
                gr.value += ">>>" + ex.Message;
                gr.messgae = ex.Message;
            }
            return Ok(JsonConvert.SerializeObject(gr));
        }

        [HttpGet("download")]
        public async Task<IActionResult> DownloadFile(string filePath)
        {
            //string[] fileExtArr = filePath.Split(".");
            // string fileExt = fileExtArr[fileExtArr.Length - 1];
            var result = await _FileRepository.GetCloudFile(filePath);
            
            return Ok(result);
        }
    }
}
