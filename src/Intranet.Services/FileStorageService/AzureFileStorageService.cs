using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.RetryPolicies;
using System.IO;
using Intranet.Services.Models;
using Microsoft.AspNetCore.Http;

namespace Intranet.Services.FileStorageService
{
    public class AzureFileStorageService
    {
        private readonly CloudStorageAccount _storageAccount;
        private readonly CloudBlobClient _client;
        private readonly CloudBlobContainer _container;

        public AzureFileStorageService()
        {
            _storageAccount = CloudStorageAccount.Parse(connectionString: "UseDevelopmentStorage=true");

            // Create a blob client.
            _client = _storageAccount.CreateCloudBlobClient();

            // Get a reference to a container named "mycontainer."
            _container = _client.GetContainerReference("man-container");
        }
        #region Get
        public async Task<MemoryStream> GetBlobAsync(String blobName)
        {
            CloudBlockBlob blockBlob = _container.GetBlockBlobReference(blobName);
            using (var memoryStream = new MemoryStream())
            {
                await blockBlob.DownloadToStreamAsync(memoryStream);
                return memoryStream;
            }

        }

        public async Task<MemoryStream> GetImageAsync(String blobName)
        {
            CloudBlockBlob blockBlob = _container.GetBlockBlobReference(blobName);
            using (var memoryStream = new MemoryStream())
            {
                await blockBlob.DownloadToStreamAsync(memoryStream);
                return memoryStream;
            }

        }
        #endregion
        #region SET
        public async Task<String> SetBlobAsync(IFormFile file)
        {
            var fileName = file.FileName;
            await _container.CreateIfNotExistsAsync();
            CloudBlockBlob blockBlob = _container.GetBlockBlobReference(fileName);

            using (var fileStream = file.OpenReadStream())
            {
                await blockBlob.UploadFromStreamAsync(fileStream);
                return fileName;
            }
        }
#endregion




    }
}