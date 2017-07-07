using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Intranet.Web.Attributes;
using System.IO;
using Microsoft.AspNetCore.Http;
using Intranet.Web.Services;

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
        public IActionResult GetFile(string filename)
        {
            try
            {
                var file = _fileStorageService.GetFile(filename);
                return PhysicalFile(file.path, file.mime); // TODO: Will (probably) not work with S3
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
        public IActionResult GetImage(string filename)
        {
            try
            {
                var image = _fileStorageService.GetImage(filename);
                return PhysicalFile(image.path, image.mime); // TODO: Will (probably) not work with S3
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
        public IActionResult GetImage(int width, int height, string filename)
        {
            try
            {
                var type = _imageService.GetImageVariantType(width, height);

                var image = _fileStorageService.GetImage(filename, type);
                return PhysicalFile(image.path, image.mime); // TODO: Will (probably) not work with S3
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
        public IActionResult Post()
        {
            try
            {
                var urls = new List<string>();
                var fileNames = new List<string>();
                var filePath = Path.GetTempPath();
                var uploads = Request.Form.Files.GroupBy(f => f.ContentType.Contains("image")).ToDictionary(x => x.Key, x => x.ToList());

                var files = uploads.Where(d => !d.Key).SelectMany(d => d.Value);
                var images = uploads.Where(d => d.Key).SelectMany(d => d.Value);

                foreach (var image in images)
                {
                    var imageUrls = _fileStorageService.SaveImage(filePath, image);
                    fileNames.Add(image.FileName);
                    urls.Add(imageUrls);
                }

                foreach (var file in files)
                {
                    var blobUrl = _fileStorageService.SaveBlob(filePath, file);
                    fileNames.Add(file.FileName);
                    urls.Add(blobUrl);
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
    }
}