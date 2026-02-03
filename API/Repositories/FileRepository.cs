using Microsoft.Extensions.Logging;
using HTSA.Models;
using System;
using HTSA.DAL.Models;
using System.Threading.Tasks;
using Azure.Storage.Files.Shares;
using Azure.Storage.Files.Shares.Models;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Collections.Generic;
//----------------------------------------------------------
//-- Author: Michael Yaacoub
//-- Email: csdmichael@gmail.com
//----------------------------------------------------------

namespace HTSA.Repositories
{
    public class FileRepository : IFileRepository
    {
        ILogger _logger;
        ApplContext _context;
        IConfiguration _configuration { get; }

        public FileRepository(ApplContext context, ILogger<FileRepository> logger, IConfiguration configuration)
        {
            _logger = logger;
            _context = context;
            _configuration = configuration;
        }

        public async Task<DynaTreeItem> BuildDynaTree(string parentPath)
        {
            DynaTreeItem di = new DynaTreeItem();
            string cloudConnectionStr = _configuration.GetConnectionString("CloudStorageAccount");
            
            ShareClient share = new ShareClient(cloudConnectionStr, parentPath);
            ShareDirectoryClient rootDir = share.GetRootDirectoryClient();
            
            di = await GetDynaTree(true, rootDir, di, parentPath);

            return di;
        }

        public async Task<BasicReponse> GetCloudFile(string filePath)
        {
            BasicReponse br = new BasicReponse();

            try
            {
                string cloudConnectionStr = _configuration.GetConnectionString("CloudStorageAccount");

                string[] pathParts = filePath.Split("/");
                string shareName = "";
                int filedepth = 0;

                if (pathParts != null && pathParts.Length > 0)
                {
                    shareName = pathParts[0];
                    filedepth = pathParts.Length - 1;

                    ShareClient share = new ShareClient(cloudConnectionStr, shareName);
                    ShareDirectoryClient rootDir = share.GetRootDirectoryClient();

                    if (await rootDir.ExistsAsync())
                    {
                        // Navigate through directories
                        for (int i = 0; i < filedepth - 1; i++)
                        {
                            rootDir = rootDir.GetSubdirectoryClient(pathParts[i + 1]);
                        }
                        
                        // Get the file
                        ShareFileClient fileClient = rootDir.GetFileClient(pathParts[filedepth]);
                        
                        var downloadResponse = await fileClient.DownloadAsync();
                        MemoryStream memoryStream = new MemoryStream();
                        await downloadResponse.Value.Content.CopyToAsync(memoryStream);
                        memoryStream.Position = 0;

                        byte[] streamAsBytes = memoryStream.ToArray();
                        string encoded64str = System.Convert.ToBase64String(streamAsBytes);

                        br.IsSuccess = true;
                        br.Message = encoded64str;
                    }
                }
            }
            catch (Exception ex)
            {
                br.IsSuccess = false;
                br.Message = ex.Message;
            }
            
            return br;
        }

        private async Task<DynaTreeItem> GetDynaTree(bool isRoot, ShareDirectoryClient rootDir, DynaTreeItem di, string shareName)
        {
            if (await rootDir.ExistsAsync())
            {
                if (isRoot)
                {
                    di.isFolder = true;
                    di.relativeFileURL = shareName;
                }
                
                await foreach (ShareFileItem item in rootDir.GetFilesAndDirectoriesAsync())
                {
                    DynaTreeItem currDI = new DynaTreeItem();

                    if (item.IsDirectory)
                    {
                        ShareDirectoryClient subDir = rootDir.GetSubdirectoryClient(item.Name);

                        currDI.title = item.Name;
                        currDI.relativeFileURL = di.relativeFileURL + "/" + currDI.title;
                        currDI.isFolder = true;
                        currDI = await GetDynaTree(false, subDir, currDI, shareName);
                        di.children.Add(currDI);
                    }
                    else
                    {
                        currDI.title = item.Name;
                        currDI.relativeFileURL = di.relativeFileURL + "/" + currDI.title;
                        currDI.isFolder = false;
                        di.children.Add(currDI);
                    }
                }
            }
            return di;
        }

        public async Task<BasicReponse> UploadFileToBlobStorage(string base64File, string containerName, string fileName, string fileExt)
        {
            BasicReponse br = new BasicReponse();

            byte[] fileBytes = Convert.FromBase64String(base64File);

            return await UploadToBlobStorage(fileBytes, containerName, fileName, fileExt);
        }

        public async Task<BasicReponse> UploadToBlobStorage(Byte[] fileBytes, string containerName, string fileName, string fileExt)
        {
            BasicReponse br = new BasicReponse();

            try
            {
                string cloudConnectionStr = _configuration.GetConnectionString("CloudStorageAccount");
                BlobServiceClient blobServiceClient = new BlobServiceClient(cloudConnectionStr);
                BlobContainerClient container = blobServiceClient.GetBlobContainerClient(containerName);
                
                BlobClient blob = container.GetBlobClient(fileName);
                using (MemoryStream ms = new MemoryStream(fileBytes))
                {
                    await blob.UploadAsync(ms, overwrite: true);
                }

                br.IsSuccess = true;
                br.Message = blob.Uri.AbsolutePath;
            }
            catch (Exception ex)
            {
                br.IsSuccess = false;
                br.Message = ex.Message;
            }
            return br;
        }
    }
}
