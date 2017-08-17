using Intranet.Services.Models;
using Intranet.Web.Common.Models.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Intranet.Services.FileStorageService
{
    public class AzureFileStorageService : IFileStorageService
    {
        public readonly String _connectionString;
        public readonly String _bucket;

        public AzureFileStorageService(IOptions<AzureStorageOptions> connectionString, IOptions<AzureStorageOptions> bucket)
        {
            _connectionString = connectionString.Value.AZUREConnectionString;//FROM secrets.json

            _bucket = bucket.Value.BucketName;//FROM secrets.json

        }

        #region Get
        public async Task<StreamWithMetadata> GetBlobAsync(String filename)
        {
            return await GetStreamFromAzureInternalAsync(filename, PathsInternal.Files);
        }

        async public Task<StreamWithMetadata> GetImageAsync(String filename)
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

            return await SetStreamToAzureInternalAsync(stream, filename, path);
        }
        #endregion

        #region Private Methods
        private async Task<string> SetStreamToAzureInternalAsync(IFormFile file, string path)
        {
            try
            {
                var filename = file.FileName;

                var _storageAccount = CloudStorageAccount.Parse(connectionString:_connectionString);
                var _client = _storageAccount.CreateCloudBlobClient();
                var _container = _client.GetContainerReference(_bucket);
                var blockBlob = _container.GetBlockBlobReference(filename);

                using (var stream = file.OpenReadStream())
                {
                    await blockBlob.UploadFromStreamAsync(stream);
                }

                return filename;
            }
            catch (StorageException ex)
            {
                var invalidCredentials =ex.RequestInformation.HttpStatusMessage != null && (ex.RequestInformation.HttpStatusCode.Equals("AuthenticationFailed") || ex.RequestInformation.HttpStatusCode.Equals("InsufficientAccountPermissions"));

                if (invalidCredentials)
                {
                    throw new Exception("Check the provided Azure Credentials.");
                }
                else
                {
                    throw new Exception("Error occurred: " + ex.Message);
                }
            }
        }

        private async Task<string> SetStreamToAzureInternalAsync(Stream stream, string filename, string path)
        {
            try
            {     
                var key = $"{path}/{filename}";
                var _storageAccount = CloudStorageAccount.Parse(connectionString: _connectionString);
                var _client = _storageAccount.CreateCloudBlobClient();
                var _container = _client.GetContainerReference(_bucket);
                var blockBlob = _container.GetBlockBlobReference(key);

                using (stream)
                {
                    await blockBlob.UploadFromStreamAsync(stream);
                }

                return key;
            }
            catch (StorageException ex)
            {
                var invalidCredentials = ex.RequestInformation.HttpStatusMessage != null && (ex.RequestInformation.HttpStatusCode.Equals("AuthenticationFailed") || ex.RequestInformation.HttpStatusCode.Equals("InsufficientAccountPermissions"));

                if (invalidCredentials)
                {
                    throw new Exception("Check the provided Azure Credentials.");
                }
                else
                {
                    throw new Exception("Error occurred: " + ex.Message);
                }
            }
        }

        private async Task<StreamWithMetadata> GetStreamFromAzureInternalAsync(string filename, string  path)
        {
            try
            {
                //for blob in AZURE, blobname is only filename, no images are found when including the path/filename
                //var key = $"{path}/{filename}"; 
                var key = $"{filename}";

                var _storageAccount = CloudStorageAccount.Parse(connectionString: _connectionString);
                var _client = _storageAccount.CreateCloudBlobClient();
                var _container = _client.GetContainerReference(_bucket);
                var blockBlob = _container.GetBlockBlobReference(key);

                var readstream = await blockBlob.OpenReadAsync();

                return new StreamWithMetadata(readstream, blockBlob.Properties.ContentType, blockBlob.Properties.ETag);
            }
            catch (StorageException ex)
            {
                if (ex.RequestInformation.HttpStatusCode == (int)System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }

                var invalidCredentials = ex.RequestInformation.HttpStatusMessage != null && (ex.RequestInformation.HttpStatusCode.Equals("AuthenticationFailed") || ex.RequestInformation.HttpStatusMessage.Equals("InsufficientAccountPermissions"));

                if (invalidCredentials)
                {
                    throw new Exception("Check the provided Azure Credentials.");
                }
                else
                {
                    throw new Exception("Error occurred: " + ex.Message);
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