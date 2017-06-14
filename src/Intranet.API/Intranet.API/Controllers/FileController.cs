using ImageSharp;
using ImageSharp.Formats;
using Intranet.API.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.FileProviders;
using Intranet.API.Models.Enums;
using System.Collections;
using Intranet.API.Services;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.StaticFiles;

namespace Intranet.API.Controllers
{
    [AllowAnonymous] // TODO: Remove
    [Produces("application/json")]
    [Route("/api/v1")]
    public class FileController : Controller
    {
        private readonly string _webRootPath;
        private readonly IImageService _imageService;

        public FileController(IHostingEnvironment env,
                               IImageService imageService)
        {
            _webRootPath = env.WebRootPath;
            _imageService = imageService;
        }

        [HttpDelete]
        [Route("{filename}")]
        public IActionResult Delete(string filename)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("blob/{filename}")]
        public IActionResult Get(string filename)
        {
            try
            {
                return PhysicalFileFromFilename(filename);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("image/{filename}")]
        public IActionResult Get(string filename, bool image = true)
        {
            try
            {
                return PhysicalFileFromFilename(filename, image);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("image/{width:int}/{height:int}/{filename}")]
        public IActionResult Get(int width, int height, string filename)
        {
            try
            {
                var type = _imageService.GetImageVariantType(width, height);

                return PhysicalFileFromFilename(filename, image: true, imageVariantType: type);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [RequestFormSizeLimit(int.MaxValue)]
        [Route("upload")]
        public IActionResult Post()
        {
            try
            {
                var urls = new List<string>();
                var filePath = Path.GetTempPath();
                var uploads = Request.Form.Files.GroupBy(f => f.ContentType.Contains("image")).ToDictionary(x => x.Key, x => x.ToList());

                var files = uploads.Where(d => !d.Key).SelectMany(d => d.Value);
                var images = uploads.Where(d => d.Key).SelectMany(d => d.Value);

                foreach (var image in images)
                {
                    var imageUrls = SaveImageToDisc(filePath, image);
                    urls.Add(imageUrls);
                }

                foreach (var file in files)
                {
                    var blobUrl = SaveBlobToDisc(filePath, file);
                    urls.Add(blobUrl);
                }

                urls = urls.Select(url => $"/api/v1{url}").ToList();

                if (urls.Count == 1)
                {
                    return Json(new { location = urls.SingleOrDefault() });
                }
                else if (urls.Any())
                {
                    return Json(new { location = urls });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        [RequestFormSizeLimit(int.MaxValue)]
        public IActionResult Put(int id, [FromBody] List<IFormFile> image)
        {
            throw new NotImplementedException();
        }

        #region Private Helper Methods

        private string SaveBlobToDisc(string filePath, IFormFile file)
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

        private string SaveImageToDisc(string filePath, IFormFile image)
        {
            var imageVariantTypes = (ImageVariantType[])Enum.GetValues(typeof(ImageVariantType));

            foreach (var imageVariantType in imageVariantTypes)
            {
                var resizedImagePath = GetImagePath(filePath, image.FileName, imageVariantType);

                // Create the destination folder tree if it doesn't already exist
                Directory.CreateDirectory(Path.GetDirectoryName(resizedImagePath));

                // Resize the image and save it to the output stream
                using (var outputStream = new FileStream(resizedImagePath, FileMode.Create))
                using (var sourceImage = Image.Load(image.OpenReadStream()))
                {
                    _imageService.CropAndResizeImage(sourceImage, imageVariantType).Save(outputStream);
                }
            }

            return $"/image/{Uri.EscapeDataString(image.FileName)}".ToLower();
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

        private IActionResult PhysicalFileFromFilename(string filename, bool image = false, ImageVariantType imageVariantType = ImageVariantType.Original)
        {
            string contentType;
            string returnPath;

            var filePath = Path.GetTempPath();

            if (image)
            {
                returnPath = GetImagePath(filePath, filename, imageVariantType);
            }
            else
            {
                returnPath = GetBlobPath(filePath, filename);
            }

            new FileExtensionContentTypeProvider().TryGetContentType(returnPath, out contentType);

            return PhysicalFile(returnPath, contentType ?? "application/octet-stream");
        }

        #endregion
    }
}
