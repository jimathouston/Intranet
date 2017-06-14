using ImageSharp;
using Intranet.API.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Intranet.API.Services
{
    public interface IImageService
    {
        ImageVariantType GetImageVariantType(int width, int height);
        Image<Rgba32> CropAndResizeImage(Image<Rgba32> image, ImageVariantType imageVariantType);
        Image<Rgba32> CropImage(Image<Rgba32> image, ImageVariantType imageVariantType);
        (int width, int height)? GetSize(ImageVariantType imageVariantType);
        (int width, int height) GetSize(ImageVariantType imageVariantType, Image<Rgba32> image);
        Image<Rgba32> ResizeImage(Image<Rgba32> image, ImageVariantType imageVariantType);
    }
}
