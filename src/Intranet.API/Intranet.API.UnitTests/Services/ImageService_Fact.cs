using ImageSharp;
using Intranet.API.Models.Enums;
using Intranet.API.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Intranet.API.UnitTests.Services
{
    public class ImageService_Fact
    {
        [Fact]
        public void ReturnNullIfNoSize()
        {
            // Assign
            var imageService = new ImageService();

            // Act
            var size = imageService.GetSize(ImageVariantType.Original);

            // Assert
            Assert.Null(size);
        }

        [Fact]
        public void ReturnImageSizeIfNoSize()
        {
            // Assign
            var imageService = new ImageService();

            var sizeStub = (width: 500, height: 250);
            var image = new Image<Rgba32>(sizeStub.width, sizeStub.height);

            // Act
            var size = imageService.GetSize(ImageVariantType.Original, image);

            // Assert
            Assert.Equal(size.width, sizeStub.width);
            Assert.Equal(size.height, sizeStub.height);
        }

        [Theory]
        [InlineData(ImageVariantType.Intranet_01_32x32, 32, 32)]
        [InlineData(ImageVariantType.Intranet_03_100x100, 100, 100)]
        [InlineData(ImageVariantType.Intranet_07_500x215, 500, 215)]
        [InlineData(ImageVariantType.Intranet_10_500x365, 500, 365)]
        public void ReturnSizeFromEnum(ImageVariantType imageType, int width, int height)
        {
            // Assign
            var imageService = new ImageService();

            // Act
            var size = imageService.GetSize(imageType);

            // Assert
            Assert.Equal(size.Value.width, width);
            Assert.Equal(size.Value.height, height);
        }

        [Fact]
        public void ReturnOriginalImage()
        {
            // Assign
            var width = 1600;
            var height = 500;

            var imageService = new ImageService();
            var image = new Image<Rgba32>(width, height);

            // Act
            var resizedImage = imageService.ResizeImage(image, ImageVariantType.Original);

            // Assert
            Assert.NotEqual(image, resizedImage);
            Assert.Equal(image.Width, resizedImage.Width);
            Assert.Equal(image.Height, resizedImage.Height);
        }

        [Theory]
        [InlineData(ImageVariantType.Intranet_01_32x32, 32, 32)]
        [InlineData(ImageVariantType.Intranet_05_300x200, 300, 200)]
        [InlineData(ImageVariantType.Intranet_07_500x215, 500, 215)]
        public void ReturnResizedImage(ImageVariantType imageType, int width, int height)
        {
            // Assign
            var imageService = new ImageService();
            var image = new Image<Rgba32>(1600, 500);

            // Act
            var resizedImage = imageService.ResizeImage(image, imageType);

            // Assert
            Assert.NotEqual(image, resizedImage);
            Assert.NotEqual(image.Width, resizedImage.Width);
            Assert.NotEqual(image.Height, resizedImage.Height);
            Assert.Equal(resizedImage.Width, width);
            Assert.Equal(resizedImage.Height, height);
        }

        [Fact]
        public void ReturnUncropedImage()
        {
            // Assign
            var width = 1600;
            var height = 500;

            var imageService = new ImageService();
            var image = new Image<Rgba32>(width, height);

            // Act
            var cropedImage = imageService.CropImage(image, ImageVariantType.Original);

            // Assert
            Assert.NotEqual(image, cropedImage);
            Assert.Equal(image.Width, cropedImage.Width);
            Assert.Equal(image.Height, cropedImage.Height);
        }

        [Theory]
        [InlineData(ImageVariantType.Intranet_05_300x200, 1500, 1000)]
        public void ReturnCropedImageWidth(ImageVariantType imageType, int width, int height)
        {
            // Assign
            var imageService = new ImageService();
            var image = new Image<Rgba32>(3200, 1000);

            // Act
            var cropedImage = imageService.CropImage(image, imageType);

            // Assert
            Assert.NotEqual(image, cropedImage);
            Assert.NotEqual(image.Width, cropedImage.Width);
            Assert.Equal(image.Height, cropedImage.Height);
            Assert.Equal(cropedImage.Width, width);
            Assert.Equal(cropedImage.Height, height);
        }

        [Theory]
        [InlineData(ImageVariantType.Intranet_01_32x32, 1000, 1000)]
        [InlineData(ImageVariantType.Intranet_04_200x200, 1000, 1000)]
        public void ReturnCropedImage(ImageVariantType imageType, int width, int height)
        {
            // Assign
            var imageService = new ImageService();
            var image = new Image<Rgba32>(3200, 1000);

            // Act
            var cropedImage = imageService.CropImage(image, imageType);

            // Assert
            Assert.NotEqual(image, cropedImage);
            Assert.NotEqual(image.Width, cropedImage.Width);
            Assert.Equal(image.Height, cropedImage.Height);
            Assert.Equal(cropedImage.Width, width);
            Assert.Equal(cropedImage.Height, height);
        }

        [Theory]
        [InlineData(ImageVariantType.Intranet_07_500x215, 1000, 430)]
        public void ReturnCropedImageHeight(ImageVariantType imageType, int width, int height)
        {
            // Assign
            var imageService = new ImageService();
            var image = new Image<Rgba32>(1000, 3200);

            // Act
            var cropedImage = imageService.CropImage(image, imageType);

            // Assert
            Assert.NotEqual(image, cropedImage);
            Assert.NotEqual(image.Height, cropedImage.Height);
            Assert.Equal(image.Width, cropedImage.Width);
            Assert.Equal(cropedImage.Width, width);
            Assert.Equal(cropedImage.Height, height);
        }

        [Fact]
        public void ReturnUntouchedImage()
        {
            // Assign
            var width = 1600;
            var height = 500;

            var imageService = new ImageService();
            var image = new Image<Rgba32>(width, height);

            // Act
            var cropedImage = imageService.CropAndResizeImage(image, ImageVariantType.Original);

            // Assert
            Assert.NotEqual(image, cropedImage);
            Assert.Equal(image.Width, cropedImage.Width);
            Assert.Equal(image.Height, cropedImage.Height);
        }

        [Fact]
        public void ReturnCropedAndResizedImage()
        {
            // Assign
            var width = 1600;
            var height = 500;

            var imageService = new ImageService();
            var image = new Image<Rgba32>(width, height);

            // Act
            var cropedImage = imageService.CropAndResizeImage(image, ImageVariantType.Intranet_13_900x390);

            // Assert
            Assert.NotEqual(cropedImage, image);
            Assert.NotEqual(cropedImage.Width, image.Width);
            Assert.NotEqual(cropedImage.Height, image.Height);
            Assert.Equal(cropedImage.Width, 900);
            Assert.Equal(cropedImage.Height, 390);
        }
    }
}
