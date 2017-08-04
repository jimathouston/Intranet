using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Intranet.Services.ImageService;
using ImageSharp;

namespace Intranet.Services.UnitTests.ImageService
{
    public class ImageService_Fact
    {
        [Fact]
        public void ReturnOriginalImage()
        {
            // Assign
            var width = 1600;
            var height = 500;

            var imageService = new Services.ImageService.ImageService();
            var image = new Image<Rgba32>(width, height);

            // Act
            var resizedImage = imageService.ResizeImage(image, width, height);

            // Assert
            Assert.NotEqual(image, resizedImage);
            Assert.Equal(image.Width, resizedImage.Width);
            Assert.Equal(image.Height, resizedImage.Height);
        }

        [Theory]
        [InlineData(32, 32)]
        [InlineData(300, 200)]
        [InlineData(500, 215)]
        public void ReturnResizedImage(int width, int height)
        {
            // Assign
            var imageService = new Services.ImageService.ImageService();
            var image = new Image<Rgba32>(1600, 500);

            // Act
            var resizedImage = imageService.ResizeImage(image, width, height);

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

            var imageService = new Services.ImageService.ImageService();
            var image = new Image<Rgba32>(width, height);

            // Act
            var cropedImage = imageService.CropImage(image, width, height);

            // Assert
            Assert.NotEqual(image, cropedImage);
            Assert.Equal(image.Width, cropedImage.Width);
            Assert.Equal(image.Height, cropedImage.Height);
        }

        [Theory]
        [InlineData(1500, 1000)]
        public void ReturnCropedImageWidth(int width, int height)
        {
            // Assign
            var imageService = new Services.ImageService.ImageService();
            var image = new Image<Rgba32>(3200, 1000);

            // Act
            var cropedImage = imageService.CropImage(image, width, height);

            // Assert
            Assert.NotEqual(image, cropedImage);
            Assert.NotEqual(image.Width, cropedImage.Width);
            Assert.Equal(image.Height, cropedImage.Height);
            Assert.Equal(cropedImage.Width, width);
            Assert.Equal(cropedImage.Height, height);
        }

        [Theory]
        [InlineData(1000, 1000)]
        [InlineData(1000, 1000)]
        public void ReturnCropedImage(int width, int height)
        {
            // Assign
            var imageService = new Services.ImageService.ImageService();
            var image = new Image<Rgba32>(3200, 1000);

            // Act
            var cropedImage = imageService.CropImage(image, width, height);

            // Assert
            Assert.NotEqual(image, cropedImage);
            Assert.NotEqual(image.Width, cropedImage.Width);
            Assert.Equal(image.Height, cropedImage.Height);
            Assert.Equal(cropedImage.Width, width);
            Assert.Equal(cropedImage.Height, height);
        }

        [Theory]
        [InlineData(1000, 430)]
        public void ReturnCropedImageHeight(int width, int height)
        {
            // Assign
            var imageService = new Services.ImageService.ImageService();
            var image = new Image<Rgba32>(1000, 3200);

            // Act
            var cropedImage = imageService.CropImage(image, width, height);

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

            var imageService = new Services.ImageService.ImageService();
            var image = new Image<Rgba32>(width, height);

            // Act
            var cropedImage = imageService.CropAndResizeImage(image, width, height);

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

            var imageService = new Services.ImageService.ImageService();
            var image = new Image<Rgba32>(width, height);

            // Act
            var cropedImage = imageService.CropAndResizeImage(image, width: 900, height: 390);

            // Assert
            Assert.NotEqual(cropedImage, image);
            Assert.NotEqual(cropedImage.Width, image.Width);
            Assert.NotEqual(cropedImage.Height, image.Height);
            Assert.Equal(cropedImage.Width, 900);
            Assert.Equal(cropedImage.Height, 390);
        }
    }
}
