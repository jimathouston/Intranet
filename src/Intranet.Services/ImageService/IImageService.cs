using ImageSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace Intranet.Services.ImageService
{
    public interface IImageService
    {
        Image<Rgba32> CropAndResizeImage(Image<Rgba32> image, int width, int height);
        Image<Rgba32> CropImage(Image<Rgba32> image, int width, int height);
        Image<Rgba32> ResizeImage(Image<Rgba32> image, int width, int height);
    }
}
