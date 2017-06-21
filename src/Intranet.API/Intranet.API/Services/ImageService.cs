using ImageSharp;
using Intranet.API.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Intranet.API.Services
{
    public class ImageService : IImageService
    {
        /// <summary>
        /// Gets the width and height from <paramref name="imageVariantType"/> and fallbacks to <paramref name="image"/>
        /// </summary>
        /// <param name="imageVariantType"></param>
        /// <param name="image"></param>
        /// <returns></returns>
        public (int width, int height) GetSize(ImageVariantType imageVariantType, Image<Rgba32> image)
        {
            return GetSize(imageVariantType) ?? (width: image.Width, height: image.Height);
        }

        /// <summary>
        /// Gets the width and height from <paramref name="imageVariantType"/>
        /// </summary>
        /// <param name="imageVariantType"></param>
        /// <returns></returns>
        public (int width, int height)? GetSize(ImageVariantType imageVariantType)
        {
            try
            {
                var raw = imageVariantType.ToString().Split('_').Last();
                var widthAndHeight = raw.Split('x').Select(size => Convert.ToInt32(size));
                return (width: widthAndHeight.First(), height: widthAndHeight.Last());
            }
            catch (Exception)
            {
                return null;
            }
        }

        // TODO: Add tests
        /// <summary>
        /// Returns an ImageVariantType based on width and height
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public ImageVariantType GetImageVariantType(int width, int height)
        {
            foreach (ImageVariantType type in Enum.GetValues(typeof(ImageVariantType)))
            {
                try
                {
                    var containsWidth = type.ToString().Split('_').Last().Split('x').First().Contains(width.ToString());
                    var containsHeight = type.ToString().Split('_').Last().Split('x').Last().Contains(height.ToString());

                    if (containsWidth && containsHeight)
                    {
                        return type;
                    }
                }
                catch (Exception)
                {
                    continue;
                }
            }

            return ImageVariantType.Original;
        }

        /// <summary>
        /// Resizes <paramref name="image"/> to <paramref name="imageVariantType"/>
        /// </summary>
        /// <param name="image"></param>
        /// <param name="imageVariantType"></param>
        /// <returns></returns>
        public Image<Rgba32> ResizeImage(Image<Rgba32> image, ImageVariantType imageVariantType)
        {
            var size = GetSize(imageVariantType, image);

            return new Image<Rgba32>(image).Resize(size.width, size.height);
        }

        /// <summary>
        /// Crops <paramref name="image"/> to match the ratio of <paramref name="imageVariantType"/>
        /// </summary>
        /// <param name="image"></param>
        /// <param name="imageVariantType"></param>
        /// <returns></returns>
        public Image<Rgba32> CropImage(Image<Rgba32> image, ImageVariantType imageVariantType)
        {
            var size = GetSize(imageVariantType, image);

            var widthRatio = (double)size.width / (double)image.Width;
            var heightRatio = (double)size.height / (double)image.Height;

            var cropWidth = heightRatio > widthRatio;
            var cropHeight = widthRatio > heightRatio;

            var cropTo = cropWidth
                ? (width: Convert.ToInt32(size.width / heightRatio), height: image.Height)
                : (width: image.Width, height: Convert.ToInt32(size.height / widthRatio));

            var sourceX = cropWidth ? (image.Width - cropTo.width) / 2 : 0;
            var sourceY = cropHeight ? (image.Height - cropTo.height) / 2 : 0;

            var cropRectangle = new Rectangle(sourceX, sourceY, cropTo.width, cropTo.height);

            return new Image<Rgba32>(image).Crop(cropRectangle);
        }

        /// <summary>
        /// First crop <paramref name="image"/> to the same ratio as <paramref name="imageVariantType"/> and then resizes to the same size
        /// </summary>
        /// <param name="image"></param>
        /// <param name="imageVariantType"></param>
        /// <returns></returns>
        public Image<Rgba32> CropAndResizeImage(Image<Rgba32> image, ImageVariantType imageVariantType)
        {
            var cropedImage = CropImage(image, imageVariantType);

            return ResizeImage(cropedImage, imageVariantType);
        }
    }
}
