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
        public async Task<StreamWithMetadata> GetBlobAsync(String filename)
        {
            return await GetStreamFromAzureInternalAsync(filename, PathsInternal.Files);

        }

        async public Task<StreamWithMetadata> GetImageAsync(string filename)
        {
            var path = GetImagePathInternal(filename);

            var streamWithMetadata = await GetStreamFromAzureInternalAsync(filename, path);

            if (streamWithMetadata!=null)
            {
                return streamWithMetadata;
            }

            // If the image didn't exist it could have been added manually to the root of the image folder
            path = $"{PathsInternal.Images}";

            return await GetStreamFromAzureInternalAsync(filename, path);
        }

        async public Task<StreamWithMetadata> GetImageAsync(string filename, int width, int height)
        {
            var path = GetImagePathInternal(filename, width, height);

            return await GetStreamFromAzureInternalAsync(filename, path);
        }



        #endregion

        #region SET

        #region SET
        async public Task<string> SetBlobAsync(IFormFile file)
        {
            return await SetStreamToAzureInternalAsync(file, PathsInternal.Files);
        }

        async public Task<string> SetImageAsync(IFormFile file)
        {
            var path = GetImagePathInternal(file.FileName);

            return await SetStreamToAzureInternalAsync(file, path);
        }

        async public Task<string> SetImageAsync(IFormFile file, int width, int height)
        {
            var path = GetImagePathInternal(file.FileName, width, height);

            return await SetStreamToAzureInternalAsync(file, path);
        }

        async public Task<string> SetImageAsync(Stream stream, string filename, int width, int height)
        {
            var path = GetImagePathInternal(filename, width, height);
            CloudBlockBlob blockBlob = _container.GetBlockBlobReference(filename);

            try
            {
                using (stream)
                {
                    await blockBlob.UploadFromStreamAsync(stream);

                    return filename;
                }
            }
            catch (StorageException WaspException)
            {
                var invalidCredentials = WaspException.RequestInformation.HttpStatusMessage != null && (
                    WaspException.RequestInformation.HttpStatusMessage.Equals("InvalidAccessKeyId") ||
                    WaspException.RequestInformation.HttpStatusMessage.Equals("InvalidSecurity"));

                if (invalidCredentials)
                {
                    throw new Exception("Check the provided Azure Credentials.");
                }
                else
                {
                    throw new Exception("Error occurred: " + WaspException.Message);
                }
            }
        }
        #endregion

        #region Private Methods

        private async Task<string> SetStreamToAzureInternalAsync(IFormFile file, string path)
        {
            try
            {
                var filename = file.FileName;
                CloudBlockBlob blockBlob = _container.GetBlockBlobReference(filename);
                using (var stream = file.OpenReadStream())
                {
                    await blockBlob.UploadFromStreamAsync(stream);

                    return filename;
                }
            }
            catch (StorageException WasbException)
            {
                var invalidCredentials = WasbException.RequestInformation.HttpStatusMessage != null && (
                    WasbException.RequestInformation.HttpStatusCode.Equals("InvalidAccessKeyId") ||
                    WasbException.RequestInformation.HttpStatusCode.Equals("InvalidSecurity"));

                if (invalidCredentials)
                {
                    throw new Exception("Check the provided Azure Credentials.");
                }
                else
                {
                    throw new Exception("Error occurred: " + WasbException.Message);
                }
            }
        }
        #endregion
        private async Task<StreamWithMetadata> GetStreamFromAzureInternalAsync(string filename, string path)
        {
            try
            {
                var key = $"{path}/{filename}";

                CloudBlockBlob blockBlob = _container.GetBlockBlobReference(key);

                var readstream = await blockBlob.OpenReadAsync();
                return new StreamWithMetadata(readstream, blockBlob.Properties.ContentType, blockBlob.Properties.ETag);

            }
            catch (StorageException WasbException)
            {

                if (WasbException.RequestInformation.HttpStatusCode == (int)System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }

                var invalidCredentials = WasbException.RequestInformation.HttpStatusMessage != null && (WasbException.RequestInformation.HttpStatusCode.Equals(
                    "InvalidAccessKeyId") || WasbException.RequestInformation.HttpStatusMessage.Equals("InvalidSecurity"));

                if (invalidCredentials)
                {
                    throw new Exception("Check the provided Azure Credentials.");
                }
                else
                {
                    throw new Exception("Error occurred: " + WasbException.Message);
                }
            }
        }
        #endregion
        #region Private Helpers
        private static string GetImagePathInternal(string filename)
        {
            return $"{PathsInternal.Images}/{Path.GetFileNameWithoutExtension(filename)}/original";
        }

        private static string GetImagePathInternal(string filename, int width, int height)
        {
            return $"{PathsInternal.Images}/{Path.GetFileNameWithoutExtension(filename)}/w_{width}-h_{height}";
        }

        private struct PathsInternal
        {
            public const string Files = "files";
            public const string Images = "images";
            public const string Original = "original";
        }
        #endregion
    }



}