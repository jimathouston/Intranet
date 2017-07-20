using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Intranet.Web.Attributes;
using System.IO;
using Microsoft.AspNetCore.Http;
using Intranet.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Intranet.Shared.Extensions;
using ImageSharp;

namespace Intranet.Web.Controllers
{
    [Produces("application/json")]
    public class FileController : Controller
    {
        private readonly IImageService _imageService;
        private readonly IFileStorageService _fileStorageService;

        public FileController(IFileStorageService fileStorageService,
                              IImageService imageService)
        {
            _fileStorageService = fileStorageService;
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
        public async Task<IActionResult> GetFile(string filename)
        {
            try
            {
                var file = await _fileStorageService.GetBlobAsync(filename);

                if (file.IsNull())
                {
                    return NotFound();
                }

                if (NotModified(file.ETag))
                {
                    return StatusCode(StatusCodes.Status304NotModified);
                }

                AddETagToHeaders(file.ETag);

                return File(file.Stream, file.ContentType);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("image/{filename}")]
        public async Task<IActionResult> GetImage(string filename)
        {
            try
            {
                var image = await _fileStorageService.GetImageAsync(filename);

                if (image.IsNull())
                {
                    return NotFound();
                }

                if (NotModified(image.ETag))
                {
                    return StatusCode(StatusCodes.Status304NotModified);
                }

                AddETagToHeaders(image.ETag);

                return File(image.Stream, image.ContentType);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("image/{width:int}/{height:int}/{filename}")]
        public async Task<IActionResult> GetImage(int width, int height, string filename)
        {
            try
            {
                var image = await _fileStorageService.GetImageAsync(filename, width, height);

                if (image.IsNotNull())
                {
                    if (NotModified(image.ETag))
                    {
                        return StatusCode(StatusCodes.Status304NotModified);
                    }

                    AddETagToHeaders(image.ETag);

                    return File(image.Stream, image.ContentType);
                }

                var originalImage = await _fileStorageService.GetImageAsync(filename);

                if (originalImage.IsNull())
                {
                    return NotFound();
                }

                var imageToResize = Image.Load(originalImage.Stream);

                var resizedImage = _imageService.CropAndResizeImage(imageToResize, width, height);

                Stream saveStream = new MemoryStream();

                var fileExtension = Path.GetExtension(filename).Replace(".", String.Empty);
                var imageFormat = Configuration.Default.FindFormatByFileExtensions(fileExtension);

                resizedImage.Save(saveStream, imageFormat);

                // Save the new image to file storage
                await _fileStorageService.SetImageAsync(saveStream, filename, width, height);

                // Stream has now been disposed so we need to get a new one from file storage
                var newImage = await _fileStorageService.GetImageAsync(filename, width, height);

                AddETagToHeaders(newImage.ETag);

                return File(newImage.Stream, newImage.ContentType);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                return BadRequest();
            }
        }

        [HttpPost]
        [RequestFormSizeLimit(int.MaxValue)]
        [Route("upload")]
        public async Task<IActionResult> Post()
        {
            try
            {
                var urls = new List<string>();
                var fileNames = new List<string>();
                var uploads = Request.Form.Files.GroupBy(f => f.ContentType.Contains("image")).ToDictionary(x => x.Key, x => x.ToList());

                var files = uploads.Where(d => !d.Key).SelectMany(d => d.Value);
                var images = uploads.Where(d => d.Key).SelectMany(d => d.Value);

                foreach (var image in images)
                {
                    var filename = await _fileStorageService.SetImageAsync(image);
                    fileNames.Add(filename);
                    urls.Add($"/image/{filename}"); // NOTE: Must match the route!
                }

                foreach (var file in files)
                {
                    var filename = await _fileStorageService.SetBlobAsync(file);
                    fileNames.Add(filename);
                    urls.Add($"/blob/{filename}"); // NOTE: Must match the route!
                }

                urls = urls.ToList();

                // The first case is to be able to upload images from TinyMCE
                if (urls.Count == 1)
                {
                    return Json(new { location = urls.SingleOrDefault(), fileName = fileNames.SingleOrDefault() });
                }
                else if (urls.Any())
                {
                    return Json(new { locations = urls, fileNames = fileNames });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
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

        #region Private Helpers
        private bool NotModified(string etag)
        {
            return HttpContext.Request.Headers.Keys.Contains("If-None-Match") && HttpContext.Request.Headers["If-None-Match"].ToString() == etag;
        }

        private void AddETagToHeaders(string etag)
        {
            HttpContext.Response.Headers.Add("ETag", new[] { etag });
        }
        #endregion
    }
}