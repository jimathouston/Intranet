using ImageSharp;
using SixLabors.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace Intranet.Services.ImageService
{
    public class ImageService : IImageService
    {
        /// <summary>
        /// Resizes <paramref name="image"/> to <paramref name="width"/>/<paramref name="height"/>
        /// </summary>
        /// <param name="image"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public Image<Rgba32> ResizeImage(Image<Rgba32> image, int width, int height)
        {
            return new Image<Rgba32>(image).Resize(width, height);
        }

        /// <summary>
        /// Crops <paramref name="image"/> to match the ratio of <paramref name="width"/>/<paramref name="height"/>
        /// </summary>
        /// <param name="image"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public Image<Rgba32> CropImage(Image<Rgba32> image, int width, int height)
        {
            var widthRatio = (double)width / (double)image.Width;
            var heightRatio = (double)height / (double)image.Height;

            var cropWidth = heightRatio > widthRatio;
            var cropHeight = widthRatio > heightRatio;

            var cropTo = cropWidth
                ? (width: Convert.ToInt32(width / heightRatio), height: image.Height)
                : (width: image.Width, height: Convert.ToInt32(height / widthRatio));

            var sourceX = cropWidth ? (image.Width - cropTo.width) / 2 : 0;
            var sourceY = cropHeight ? (image.Height - cropTo.height) / 2 : 0;

            var cropRectangle = new Rectangle(sourceX, sourceY, cropTo.width, cropTo.height);

            return new Image<Rgba32>(image).Crop(cropRectangle);
        }

        /// <summary>
        /// First crop <paramref name="image"/> to the same ratio as <paramref name="width"/>/<paramref name="height"/> and then resizes to the same size
        /// </summary>
        /// <param name="image"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public Image<Rgba32> CropAndResizeImage(Image<Rgba32> image, int width, int height)
        {
            var cropedImage = CropImage(image, width, height);

            return ResizeImage(cropedImage, width, height);
        }
    }
}
