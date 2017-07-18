using Intranet.Web.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace Intranet.Web.Services
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