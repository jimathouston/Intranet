using System;
using System.Collections.Generic;
using System.Text;
using Intranet.API.Domain.Models.Entities;
using Xunit;
using Moq;
using Intranet.API.Domain.Data;
using Intranet.API.UnitTests.Mocks;
using Intranet.API.Controllers;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Intranet.API.UnitTests.Controllers
{
  public class NewsController_Fact
  {
    [Theory]
    [InlineData(1, "2017-04-03 02:00:00", "Rubrik 1", "Detta är en text till nyhet 1", "Charlotta Utterström")]
    public void ReturnAllNewsCorrectly(int newsId, string newsDate, string newsTitle, string newsText, string newsAuthor)
    {
      // Assign
      var utcDate = new DateTimeOffset(Convert.ToDateTime(newsDate));
      var news = GetFakeNews(newsId, utcDate, newsTitle, newsText, newsAuthor);
      var mockSet = DbSetMock.MockSet(news);

      var mockContext = new Mock<IntranetApiContext>();
      mockContext.Setup(m => m.News).Returns(mockSet.Object);
      var newsController = new NewsController(mockContext.Object);

      // Act
      var fetchNews = newsController.Get();
      var newsFromController = fetchNews.SingleOrDefault();

      // Assert
      Assert.Equal(newsFromController.Id, newsId);
      Assert.Equal(newsFromController.Date, utcDate);
      Assert.Equal(newsFromController.Title, newsTitle);
      Assert.Equal(newsFromController.Text, newsText);
      Assert.Equal(newsFromController.Author, newsAuthor);
    }
    
    [Fact]
    public void ReturnSpecificNewsById()
    {
      // Assign
      var id = 1;
      var news = GetFakeNews();
      var mockSet = DbSetMock.MockSet(news);

      var mockContext = new Mock<IntranetApiContext>();
      mockContext.Setup(m => m.News).Returns(mockSet.Object);
      var newsController = new NewsController(mockContext.Object);

      // Act
      var result = newsController.Get(id);
      var okObjectResult = result as OkObjectResult;
      var newsContent = okObjectResult.Value as News;

      // Assert
      Assert.NotNull(okObjectResult);
      Assert.Equal(id, newsContent.Id);
    }

    [Fact]
    public void ReturnStatusOKWhenSearchById()
    {
      // Assign
      int id = 1;
      int statusCode = 200;
      var news = GetFakeNews();
      var mockSet = DbSetMock.MockSet(news);

      var mockContext = new Mock<IntranetApiContext>();
      mockContext.Setup(m => m.News).Returns(mockSet.Object);
      var newsController = new NewsController(mockContext.Object);

      // Act
      var result = newsController.Get(id);

      // Assert
      var obj = (OkObjectResult)result;
      Assert.True(obj.StatusCode == statusCode);
    }

    [Fact]
    public void ReturnStatusNotFoundWhenSearchById()
    {
      // Assign
      int id = 2;
      int statusCode = 404;
      var news = GetFakeNews();
      var mockSet = DbSetMock.MockSet(news);

      var mockContext = new Mock<IntranetApiContext>();
      mockContext.Setup(m => m.News).Returns(mockSet.Object);
      var newsController = new NewsController(mockContext.Object);

      // Act
      var result = newsController.Get(id);

      // Assert
      var obj = (NotFoundResult)result;
      Assert.True(obj.StatusCode == statusCode);
    }

    [Theory]
    [InlineData(false, 200)]
    public void ReturnCorrectStatusCode(bool shouldBeNull, int statusCode) // TODO not able to test status code, remove or invest further?
    {
      // Assign
      var news = new List<News>();
      var mockSet = DbSetMock.MockSet(news);
      var mockContext = new Mock<IntranetApiContext>();

      mockContext.Setup(m => m.News).Returns(shouldBeNull ? null : mockSet.Object);

      var newsController = new NewsController(mockContext.Object);

      // Act
      var newsFromController = newsController.Get();
      
      // Assert
      Assert.Equal(newsFromController, shouldBeNull ? null : news);
    }

    private IEnumerable<News> GetFakeNews()
    {
      return GetFakeNews(newsDate: DateTimeOffset.Now);
    }

    private IEnumerable<News> GetFakeNews(DateTimeOffset newsDate)
    {
      return GetFakeNews
        (
          newsId: 1,
          newsDate: newsDate,
          newsTitle: "Rubrik 1",
          newsText: "Detta är en text till nyhet 1",
          newsAuthor: "Charlotta Utterström"
        );
    }

    /// <summary>
    /// Generate and return dummy news 
    /// </summary>
    /// <returns></returns>
    private IEnumerable<News> GetFakeNews(int newsId, DateTimeOffset newsDate, string newsTitle, string newsText, string newsAuthor)
    {
      return new News[]
      {
        new News
        {
          Id = newsId,
          Date = newsDate,
          Title = newsTitle,
          Text = newsText,
          Author = newsAuthor
        }
      };
    }
  }
}
