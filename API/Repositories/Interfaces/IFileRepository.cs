using HTSA.DAL.Models;
using HTSA.Models;
using System;
using System.Threading.Tasks;

namespace HTSA.Repositories
{
    public interface IFileRepository
    {
        Task<BasicReponse> UploadFileToBlobStorage(string base64File, string containerName, string fileName, string fileExt);
        Task<BasicReponse> UploadToBlobStorage(Byte[] fileBytes, string containerName, string fileName, string fileExt);
        Task<DynaTreeItem> BuildDynaTree(string parentPath);
        Task<BasicReponse> GetCloudFile(string filePath);
    }
}