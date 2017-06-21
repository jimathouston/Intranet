using Intranet.API.Common.Enums;
using Intranet.API.Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Intranet.API.UnitTests.Models.Entities
{
    public class Image_Fact
    {
        [Fact]
        public void ReturnUrls()
        {
            // Assign
            var image = new Image();
            var variants = (ImageVariantType[])Enum.GetValues(typeof(ImageVariantType));
            var size = variants.Last().ToString().Split('_').Last().Split('x');
            var width = size.First();
            var height = size.Last();

            image.FileName = "myimage.jpeg";

            var count = variants.Length;

            // Act
            var urls = image.Urls;

            // Assert
            Assert.Equal(urls.Count(), count);
            Assert.Equal(urls.First(), $"/api/v1/image/{image.FileName}");
            Assert.Equal(urls.Last(), $"/api/v1/image/{width}/{height}/{image.FileName}");
        }

        [Fact]
        public void ReturnNull()
        {
            // Assign
            var image = new Image();

            // Act
            var urls = image.Urls;

            // Assert
            Assert.Equal(urls.Single(), null);
        }
    }
}
