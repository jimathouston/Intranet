using Intranet.Services.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Intranet.Services.FileStorageService
{
    public interface IFileStorageService
    {
        Task<StreamWithMetadata> GetBlobAsync(string filename);
        Task<StreamWithMetadata> GetImageAsync(string filename);
        Task<StreamWithMetadata> GetImageAsync(string filename, int width, int height);
        Task<string> SetBlobAsync(IFormFile file);
        Task<string> SetImageAsync(IFormFile image);
        Task<string> SetImageAsync(IFormFile image, int width, int height);
        Task<string> SetImageAsync(Stream image, string filename, int width, int height);
    }
}
