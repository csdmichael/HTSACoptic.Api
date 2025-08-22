using Microsoft.Extensions.Logging;
using HTSA.Models;
using System;
using HTSA.DAL.Models;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.File;
using System.IO;
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
        IConfigurationRoot _configuration { get; }

        public FileRepository(ApplContext context, ILogger<FileRepository> logger, IConfigurationRoot configuration)
        {
            _logger = logger;
            _context = context;
            _configuration = configuration;
        }

        public async Task<DynaTreeItem> BuildDynaTree(string parentPath)
        {
            DynaTreeItem di = new DynaTreeItem();
            string cloudConnectionStr = _configuration.GetConnectionString("CloudStorageAccount");
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(cloudConnectionStr);

            // Create a CloudFileClient object for credentialed access to Azure Files.
            CloudFileClient fileClient = storageAccount.CreateCloudFileClient();

            // Get a reference to the file share we created previously.
            CloudFileShare share = fileClient.GetShareReference(parentPath);

            // Ensure that the share exists.

            // Get a reference to the root directory for the share.
            CloudFileDirectory rootDir = share.GetRootDirectoryReference();
            di = await GetDynaTree(true, rootDir, di);
            

            /*
            title = fsi.Name;
            children = new List<DynaTreeItem>();

            if (parentPath == "") relativeFileURL = "~#~";
            else relativeFileURL = parentPath.Replace("~#~", "") + "/" + title;

            if (fsi.Attributes == FileAttributes.Directory)
            {
                isFolder = true;
                foreach (FileSystemInfo f in (fsi as DirectoryInfo).GetFileSystemInfos())
                {

                    children.Add(new DynaTreeItem(f, relativeFileURL));
                }
            }
            else
            {
                isFolder = false;
            }
            key = title.Replace(" ", "").ToLower();
            */

            return di;
        }

        public async Task<BasicReponse> GetCloudFile(string filePath)
        {
            BasicReponse br = new BasicReponse();

            try
            {
                CloudFile cloudFile = null;
                string cloudConnectionStr = _configuration.GetConnectionString("CloudStorageAccount");
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(cloudConnectionStr);

                // Create a CloudFileClient object for credentialed access to Azure Files.
                CloudFileClient fileClient = storageAccount.CreateCloudFileClient();

                string[] pathParts = filePath.Split("/");
                string shareName = "";
                int filedepth = 0;

                if (pathParts != null & pathParts.Length > 0)
                {
                    shareName = pathParts[0];
                    filedepth = pathParts.Length - 1;

                    // Get a reference to the file share we created previously.
                    CloudFileShare share = fileClient.GetShareReference(shareName);

                    // Ensure that the share exists.

                    // Get a reference to the root directory for the share.
                    CloudFileDirectory rootDir = share.GetRootDirectoryReference();

                    if (await rootDir.ExistsAsync())
                    {
                        FileContinuationToken continuationToken = null;
                        FileResultSegment items;
                        for (int i = 0; i < filedepth - 1; i++)
                        {
                            items = await rootDir.ListFilesAndDirectoriesSegmentedAsync(continuationToken);
                            continuationToken = items.ContinuationToken;
                            foreach (IListFileItem fileItem in items.Results)
                            {
                                if (fileItem is CloudFileDirectory && ((CloudFileDirectory)fileItem).Name == pathParts[i + 1])
                                {
                                    rootDir = (CloudFileDirectory)fileItem;
                                    break;
                                }
                            }
                        }
                        items = await rootDir.ListFilesAndDirectoriesSegmentedAsync(continuationToken);
                        continuationToken = items.ContinuationToken;
                        foreach (IListFileItem fileItem in items.Results)
                        {
                            if (fileItem is CloudFile && ((CloudFile)fileItem).Name == pathParts[filedepth])
                            {
                                cloudFile = (CloudFile)fileItem;
                                break;
                            }
                        }


                    }
                }

                MemoryStream memoryStream = new MemoryStream();
                await cloudFile.DownloadToStreamAsync(memoryStream).ConfigureAwait(false);

                byte[] streamAsBytes = memoryStream.ToArray();
                string encoded64str = System.Convert.ToBase64String(streamAsBytes);

                br.IsSuccess = true;
                br.Message = encoded64str;
            }
            catch (Exception ex)
            {
                br.IsSuccess = false;
                br.Message = ex.Message;
            }
            
            return br;
        }


        private async Task<DynaTreeItem> GetDynaTree(bool isRoot, CloudFileDirectory rootDir, DynaTreeItem di)
        {
            FileResultSegment items;

            if (await rootDir.ExistsAsync())
            {
                if (isRoot)
                {
                    di.isFolder = true;
                    di.relativeFileURL = rootDir.Name;
                }
                FileContinuationToken continuationToken = null;
                do
                {
                    items = await rootDir.ListFilesAndDirectoriesSegmentedAsync(continuationToken);
                    continuationToken = items.ContinuationToken;
                    foreach (IListFileItem fileItem in items.Results)
                    {
                        DynaTreeItem currDI = new DynaTreeItem();

                        if (fileItem is CloudFileDirectory)
                        {
                            CloudFileDirectory cloudDir = fileItem as CloudFileDirectory;

                            currDI.title = cloudDir.Name;
                            currDI.relativeFileURL = di.relativeFileURL + "/" + currDI.title;
                            currDI.isFolder = true;
                            currDI = await GetDynaTree(false, cloudDir, currDI);
                            di.children.Add(currDI);
                        }
                        else if (fileItem is CloudFile)
                        {
                            CloudFile cloudFile = fileItem as CloudFile;

                            currDI.title = cloudFile.Name;
                            currDI.relativeFileURL = di.relativeFileURL + "/" + currDI.title;
                            //currDI.relativeFileURL = cloudFile.Uri.AbsoluteUri;
                            currDI.isFolder = false;
                            di.children.Add(currDI);
                        }

                        
                    }
                }
                while (continuationToken != null);
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
                CloudStorageAccount storageAccount = null;
                if (CloudStorageAccount.TryParse(_configuration.GetConnectionString("CloudStorageAccount"), out storageAccount))
                {
                    var client = storageAccount.CreateCloudBlobClient();
                    var container = client.GetContainerReference(containerName);

                    CloudBlockBlob blob = container.GetBlockBlobReference(fileName);
                    // blob.Properties.ContentType = fileType;
                    await blob.UploadFromByteArrayAsync(fileBytes, 0, fileBytes.Length);

                    br.IsSuccess = true;
                    br.Message = blob.StorageUri.PrimaryUri.AbsolutePath;
                }


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
