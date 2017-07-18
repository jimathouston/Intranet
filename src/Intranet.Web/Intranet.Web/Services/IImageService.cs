using ImageSharp;

namespace Intranet.Web.Services
{
    public interface IImageService
    {
        Image<Rgba32> CropAndResizeImage(Image<Rgba32> image, int width, int height);
        Image<Rgba32> CropImage(Image<Rgba32> image, int width, int height);
        Image<Rgba32> ResizeImage(Image<Rgba32> image, int width, int height);
    }
}