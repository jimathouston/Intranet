using ImageSharp;
using Intranet.Shared.Common.Enums;

namespace Intranet.Web.Services
{
    public interface IImageService
    {
        Image<Rgba32> CropAndResizeImage(Image<Rgba32> image, ImageVariantType imageVariantType);
        Image<Rgba32> CropImage(Image<Rgba32> image, ImageVariantType imageVariantType);
        ImageVariantType GetImageVariantType(int width, int height);
        (int width, int height)? GetSize(ImageVariantType imageVariantType);
        (int width, int height) GetSize(ImageVariantType imageVariantType, Image<Rgba32> image);
        Image<Rgba32> ResizeImage(Image<Rgba32> image, ImageVariantType imageVariantType);
    }
}