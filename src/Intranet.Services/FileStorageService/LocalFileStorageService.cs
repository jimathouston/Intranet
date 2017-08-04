using Intranet.Services.ImageService;
using Intranet.Services.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Intranet.Services.FileStorageService
{
    public class LocalFileStorageService : IFileStorageService
    {
        private readonly IImageService _imageService;

        public LocalFileStorageService(IImageService imageService)
        {
            _imageService = imageService;
        }

        public async Task<string> SetBlobAsync(IFormFile file)
        {
            var blobPath = GetBlobPath(file.FileName);

            // Create the destination folder tree if it doesn't already exist
            Directory.CreateDirectory(Path.GetDirectoryName(blobPath));

            using (var outputStream = new FileStream(blobPath, FileMode.Create))
            using (var sourceFile = file.OpenReadStream())
            {
                await sourceFile.CopyToAsync(outputStream);
            }

            return file.FileName;
        }

        public async Task<string> SetImageAsync(IFormFile image)
        {
            var imagePath = GetImagePath(image.FileName);

            // Create the destination folder tree if it doesn't already exist
            Directory.CreateDirectory(Path.GetDirectoryName(imagePath));

            using (var outputStream = new FileStream(imagePath, FileMode.Create))
            using (var sourceFile = image.OpenReadStream())
            {
                await sourceFile.CopyToAsync(outputStream);
            }

            return image.FileName;
        }

        public async Task<string> SetImageAsync(IFormFile image, int width, int height)
        {
            var imagePath = GetImagePath(image.FileName, width, height);

            // Create the destination folder tree if it doesn't already exist
            Directory.CreateDirectory(Path.GetDirectoryName(imagePath));

            using (var outputStream = new FileStream(imagePath, FileMode.Create))
            using (var sourceFile = image.OpenReadStream())
            {
                await sourceFile.CopyToAsync(outputStream);
            }

            return image.FileName;
        }

        // TODO: Broken, image stream seems to be empty?
        public async Task<string> SetImageAsync(Stream image, string filename, int width, int height)
        {
            var imagePath = GetImagePath(filename, width, height);

            // Create the destination folder tree if it doesn't already exist
            Directory.CreateDirectory(Path.GetDirectoryName(imagePath));

            using (var outputStream = new FileStream(imagePath, FileMode.Create))
            using (image)
            {
                await image.CopyToAsync(outputStream);
            }

            return filename;
        }

        public async Task<StreamWithMetadata> GetBlobAsync(string filename)
        {
            var filePath = GetBlobPath(filename);
            return StreamWithContentTypeInternal(filePath);
        }

        public async Task<StreamWithMetadata> GetImageAsync(string filename)
        {
            var filePath = GetImagePath(filename);
            return StreamWithContentTypeInternal(filePath);
        }

        public async Task<StreamWithMetadata> GetImageAsync(string filename, int width, int height)
        {
            var filePath = GetImagePath(filename, width, height);
            return StreamWithContentTypeInternal(filePath);
        }

        #region Private Helpers
        private static StreamWithMetadata StreamWithContentTypeInternal(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return null;
            }

            var stream = File.OpenRead(filePath);

            new FileExtensionContentTypeProvider().TryGetContentType(filePath, out string contentType);

            return new StreamWithMetadata(stream, contentType ?? "application/octet-stream", null);
        }

        private static string GetBlobPath(string filename)
        {
            return Path.Combine(Path.GetTempPath(), "intranet", "blobs", filename);
        }

        private string GetImagePath(string filename)
        {
            return Path.Combine(Path.GetTempPath(),
                                "intranet",
                                "images",
                                Path.GetFileNameWithoutExtension(filename),
                                "original",
                                filename);
        }

        private string GetImagePath(string filename, int width, int height)
        {
            return Path.Combine(Path.GetTempPath(),
                    "intranet",
                    "images",
                    Path.GetFileNameWithoutExtension(filename),
                    $"w_{width}-h_{height}",
                    filename);
        }
        #endregion
    }
}
