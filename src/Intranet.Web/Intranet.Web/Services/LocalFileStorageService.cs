using ImageSharp;
using Intranet.Shared.Common.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Intranet.Web.Services
{
    public class LocalFileStorageService : IFileStorageService
    {
        private readonly IImageService _imageService;

        public LocalFileStorageService(IImageService imageService)
        {
            _imageService = imageService;
        }

        public string SaveBlob(string filePath, IFormFile file)
        {
            var imageVariantTypes = (ImageVariantType[])Enum.GetValues(typeof(ImageVariantType));

            var blobPath = GetBlobPath(filePath, file.FileName);

            // Create the destination folder tree if it doesn't already exist
            Directory.CreateDirectory(Path.GetDirectoryName(blobPath));

            using (var outputStream = new FileStream(blobPath, FileMode.Create))
            using (var sourceFile = file.OpenReadStream())
            {
                sourceFile.CopyTo(outputStream);
            }

            return $"/blob/{Uri.EscapeDataString(file.FileName)}".ToLower();
        }

        public string SaveImage(string filePath, IFormFile image)
        {
            var imageVariantTypes = (ImageVariantType[])Enum.GetValues(typeof(ImageVariantType));

            foreach (var imageVariantType in imageVariantTypes)
            {
                var resizedImagePath = GetImagePath(filePath, image.FileName, imageVariantType);

                // Create the destination folder tree if it doesn't already exist
                Directory.CreateDirectory(Path.GetDirectoryName(resizedImagePath));

                // Resize the image and save it to the output stream
                using (var sourceImage = Image.Load(image.OpenReadStream()))
                {
                    _imageService.CropAndResizeImage(sourceImage, imageVariantType).Save(resizedImagePath);
                }
            }

            return $"/image/{Uri.EscapeDataString(image.FileName)}".ToLower();
        }

        public (string path, string mime) GetFile(string filename)
        {
            string contentType;

            var filePath = Path.GetTempPath();
            var returnPath = GetBlobPath(filePath, filename);

            new FileExtensionContentTypeProvider().TryGetContentType(returnPath, out contentType);

            //return PhysicalFile(returnPath, contentType ?? "application/octet-stream");
            return (returnPath, contentType ?? "application/octet-stream");
        }

        public (string path, string mime) GetImage(string filename, ImageVariantType imageVariantType = ImageVariantType.Original)
        {
            string contentType;

            var filePath = Path.GetTempPath();

            var returnPath = GetImagePath(filePath, filename, imageVariantType);

            new FileExtensionContentTypeProvider().TryGetContentType(returnPath, out contentType);

            //return PhysicalFile(returnPath, contentType ?? "application/octet-stream");
            return (returnPath, contentType ?? "application/octet-stream");
        }

        private static string GetBlobPath(string filePath, string filename)
        {
            return Path.Combine(filePath, "intranet", "blobs", filename);
        }

        private static string GetImagePath(string filePath, string filename, ImageVariantType imageVariantType)
        {
            return Path.Combine(filePath,
                                "intranet",
                                "images",
                                Path.GetFileNameWithoutExtension(filename),
                                imageVariantType.ToString(),
                                filename);
        }
    }
}
