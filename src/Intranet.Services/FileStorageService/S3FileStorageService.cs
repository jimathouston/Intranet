using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Intranet.Services.Models;
using Intranet.Web.Common.Extensions;
using Intranet.Web.Common.Models.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Intranet.Services.FileStorageService
{
    public class S3FileStorageService : IFileStorageService
    {
        private readonly IAmazonS3 _s3Client;
        private readonly S3Options _settings;
        private readonly ITransferUtility _transferUtility;

        public S3FileStorageService(IAmazonS3 s3Client,
                                    IOptions<S3Options> settings,
                                    ITransferUtility transferUtility)
        {
            _transferUtility = transferUtility;
            _settings = settings.Value;
            _s3Client = s3Client;
        }

        #region GET
        async public Task<StreamWithMetadata> GetBlobAsync(String filename)
        {
            return await GetStreamFromS3InternalAsync(filename, PathsInternal.Files);
        }

        async public Task<StreamWithMetadata> GetImageAsync(string filename)
        {
            var path = GetImagePathInternal(filename);

            var streamWithMetadata = await GetStreamFromS3InternalAsync(filename, path);

            if (streamWithMetadata.IsNotNull())
            {
                return streamWithMetadata;
            }

            // If the image didn't exist it could have been added manually to the root of the image folder
            path = $"{PathsInternal.Images}";

            return await GetStreamFromS3InternalAsync(filename, path);
        }

        async public Task<StreamWithMetadata> GetImageAsync(string filename, int width, int height)
        {
            var path = GetImagePathInternal(filename, width, height);

            return await GetStreamFromS3InternalAsync(filename, path);
        }
        #endregion

        #region SET
        async public Task<string> SetBlobAsync(IFormFile file)
        {
            return await SetStreamToS3InternalAsync(file, PathsInternal.Files);
        }

        async public Task<string> SetImageAsync(IFormFile file)
        {
            var path = GetImagePathInternal(file.FileName);

            return await SetStreamToS3InternalAsync(file, path);
        }

        async public Task<string> SetImageAsync(IFormFile file, int width, int height)
        {
            var path = GetImagePathInternal(file.FileName, width, height);

            return await SetStreamToS3InternalAsync(file, path);
        }

        async public Task<string> SetImageAsync(Stream stream, string filename, int width, int height)
        {
            var path = GetImagePathInternal(filename, width, height);

            try
            {
                using (stream)
                {
                    await _transferUtility.UploadAsync(stream, _settings.BucketName, $"{path}/{filename}");

                    return filename;
                }
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                var invalidCredentials = amazonS3Exception.ErrorCode != null && (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") || amazonS3Exception.ErrorCode.Equals("InvalidSecurity"));

                if (invalidCredentials)
                {
                    throw new Exception("Check the provided AWS Credentials.");
                }
                else
                {
                    throw new Exception("Error occurred: " + amazonS3Exception.Message);
                }
            }
        }
        #endregion

        #region Private Methods
        private async Task<StreamWithMetadata> GetStreamFromS3InternalAsync(string filename, string path)
        {
            try
            {
                var key = $"{path}/{filename}";

                var request = new GetObjectRequest
                {
                    BucketName = _settings.BucketName,
                    Key = key,
                };

                var getObjectResponse = await _s3Client.GetObjectAsync(request);
                return new StreamWithMetadata(getObjectResponse.ResponseStream, getObjectResponse.Headers.ContentType, getObjectResponse.ETag);
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }

                var invalidCredentials = amazonS3Exception.ErrorCode != null && (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") || amazonS3Exception.ErrorCode.Equals("InvalidSecurity"));

                if (invalidCredentials)
                {
                    throw new Exception("Check the provided AWS Credentials.");
                }
                else
                {
                    throw new Exception("Error occurred: " + amazonS3Exception.Message);
                }
            }
        }

        private async Task<string> SetStreamToS3InternalAsync(IFormFile file, string path)
        {
            try
            {
                var filename = file.FileName;

                using (var stream = file.OpenReadStream())
                {
                    await _transferUtility.UploadAsync(stream, _settings.BucketName, $"{path}/{filename}");

                    return filename;
                }
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                var invalidCredentials = amazonS3Exception.ErrorCode != null && (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") || amazonS3Exception.ErrorCode.Equals("InvalidSecurity"));

                if (invalidCredentials)
                {
                    throw new Exception("Check the provided AWS Credentials.");
                }
                else
                {
                    throw new Exception("Error occurred: " + amazonS3Exception.Message);
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
