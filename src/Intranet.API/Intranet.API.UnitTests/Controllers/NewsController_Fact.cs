using Intranet.API.Controllers;
using Intranet.API.Domain.Data;
using Intranet.API.Domain.Models.Entities;
using Intranet.API.UnitTests.Mocks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Intranet.API.UnitTests.Controllers
{
  public class NewsController_Fact
  {
    [Fact]
    public void ReturnBadRequestResultWhenPosting()
    {
      // Assign
      var options = new DbContextOptionsBuilder<IntranetApiContext>()
          .UseInMemoryDatabase()
          .Options;

      News newsItem = GetFakeNews().First();

      var context = new IntranetApiContext(options);
      context.Database.EnsureDeleted();
      var newsController = new NewsController(context);

      // Act
      newsController.ModelState.AddModelError("Author", "Author must be specified");
      var result = newsController.Post(newsItem);
      context.Dispose();

      // Assert
      var badReqResult = Assert.IsType<BadRequestObjectResult>(result);
      Assert.IsType<SerializableError>(badReqResult.Value);
    }

    [Fact]
    public void ReturnOkResultWhenPosting()
    {
      // Assign
      var options = new DbContextOptionsBuilder<IntranetApiContext>()
          .UseInMemoryDatabase()
          .Options;

      var newsItem = GetFakeNews().First();

      var context = new IntranetApiContext(options);
      context.Database.EnsureDeleted();
      var newsController = new NewsController(context);

      // Act
      var result = newsController.Post(newsItem);
      context.Dispose();

      // Assert
      Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void CheckNewsItemWasCorrectlyPosted()
    {
      // Assign
      var options = new DbContextOptionsBuilder<IntranetApiContext>()
          .UseInMemoryDatabase()
          .Options;

      var newsItem = GetFakeNews().First();

      var context = new IntranetApiContext(options);
      context.Database.EnsureDeleted();
      var newsController = new NewsController(context);

      // Act
      newsController.Post(newsItem);
      var news = context.News.First();

      // Assert
      Assert.NotNull(news);
      Assert.True(news.Text == newsItem.Text);
      Assert.True(news.Title == newsItem.Title);
      Assert.True(news.Author == newsItem.Author);
    }

    [Fact]
    public void ReturnBadRequestResultWhenUpdate()
    {
      // Assign
      var options = new DbContextOptionsBuilder<IntranetApiContext>()
          .UseInMemoryDatabase()
          .Options;

      var newsItem = GetFakeNews();
      int id = 1;

      var context = new IntranetApiContext(options);
      context.Database.EnsureDeleted();
      context.News.AddRange(newsItem);
      context.SaveChanges();

      var newsController = new NewsController(context);

      // Act
      newsController.ModelState.AddModelError("Title", "Title must be specified");
      var result = newsController.Put(id, newsItem.First());
      context.Dispose();

      // Assert
      var badReqResult = Assert.IsType<BadRequestObjectResult>(result);
      Assert.IsType<SerializableError>(badReqResult.Value);
    }

    [Fact]
    public void ReturnNotFoundWhenUpdate()
    {
      // Assign
      var options = new DbContextOptionsBuilder<IntranetApiContext>()
          .UseInMemoryDatabase()
          .Options;

      var newsItem = GetFakeNews();
      int id = 5;

      var context = new IntranetApiContext(options);
      context.Database.EnsureDeleted();
      context.News.AddRange(newsItem);
      context.SaveChanges();

      var newsController = new NewsController(context);

      // Act
      var result = newsController.Put(id, newsItem.First());
      context.Dispose();

      // Assert
      Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public void ReturnOkResultWhenUpdate()
    {
      // Assign
      var options = new DbContextOptionsBuilder<IntranetApiContext>()
          .UseInMemoryDatabase()
          .Options;

      var oldNewsItem = GetFakeNews().First();

      var newNewsItem = GetFakeNews().First();
      newNewsItem.Author = "Martin Norén";

      var context = new IntranetApiContext(options);
      context.Database.EnsureDeleted();
      context.Add(oldNewsItem);
      context.SaveChanges();

      var newsController = new NewsController(context);

      // Act
      var result = newsController.Put(1, newNewsItem);
      context.Dispose();

      // Assert
      Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void ReturnNewsById()
    {
      // Assign
      var id = 1;
      var news = GetFakeNews();
      var mockSet = DbSetMock.MockSet(news, nameof(News.NewsId));

      var mockContext = new Mock<IntranetApiContext>();
      mockContext.Setup(m => m.News).Returns(mockSet.Object);
      var newsController = new NewsController(mockContext.Object);

      // Act
      var result = newsController.Get(id);
      var okObjectResult = result as OkObjectResult;
      var newsContent = okObjectResult.Value as News;

      // Assert
      Assert.NotNull(okObjectResult);
      Assert.Equal(id, newsContent.NewsId);
    }

    [Fact]
    public void ReturnOkObjectResultWhenSearchById()
    {
      // Assign
      int id = 1;
      var news = GetFakeNews();
      var mockSet = DbSetMock.MockSet(news, nameof(News.NewsId));

      var mockContext = new Mock<IntranetApiContext>();
      mockContext.Setup(m => m.News).Returns(mockSet.Object);
      var newsController = new NewsController(mockContext.Object);

      // Act
      var result = newsController.Get(id);

      // Assert
      Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void ReturnNotFoundResultWhenSearchById()
    {
      // Assign
      int id = 2;
      var news = GetFakeNews();
      var mockSet = DbSetMock.MockSet(news, nameof(News.NewsId));

      var mockContext = new Mock<IntranetApiContext>();
      mockContext.Setup(m => m.News).Returns(mockSet.Object);
      var newsController = new NewsController(mockContext.Object);

      // Act
      var result = newsController.Get(id);

      // Assert
      Assert.IsType<NotFoundResult>(result);
    }

    [Theory]
    [InlineData(1, "2017-04-03 02:00:00", "Rubrik 1", "Detta är en text till nyhet 1", "Charlotta Utterström")]
    public void ReturnCorrectNewsInfoWhenGetAllNews(int newsId, string newsDate, string newsTitle, string newsText, string newsAuthor)
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
      var okObjectResult = fetchNews as OkObjectResult;
      var iEnumerable = okObjectResult.Value as IEnumerable<News>;
      var newsFromController = iEnumerable.First();

      // Assert
      Assert.Equal(newsFromController.NewsId, newsId);
      Assert.Equal(newsFromController.Date, utcDate);
      Assert.Equal(newsFromController.Title, newsTitle);
      Assert.Equal(newsFromController.Text, newsText);
      Assert.Equal(newsFromController.Author, newsAuthor);
    }

    [Fact]
    public void ReturnNotFoundWhenGetAllNews()
    {
      // Assign
      var options = new DbContextOptionsBuilder<IntranetApiContext>()
          .UseInMemoryDatabase()
          .Options;

      var context = new IntranetApiContext(options);
      context.Database.EnsureDeleted();
      var newsController = new NewsController(context);

      // Act
      var newsFromController = newsController.Get();
      context.Dispose();

      // Assert
      Assert.IsType<NotFoundObjectResult>(newsFromController);
    }

    [Fact]
    public void ReturnOkObjectResultWhenGetAllNews()
    {
      // Assign
      var options = new DbContextOptionsBuilder<IntranetApiContext>()
          .UseInMemoryDatabase()
          .Options;

      var newsItem = GetFakeNews();

      var context = new IntranetApiContext(options);
      context.Database.EnsureDeleted();

      context.News.Add(newsItem.First());
      context.SaveChanges();

      var newsController = new NewsController(context);

      // Act
      var newsFromController = newsController.Get();
      context.Dispose();

      // Assert
      Assert.IsType<OkObjectResult>(newsFromController);
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
          newsTitle: "News title 1",
          newsText: "This is a content placeholder for news title 1",
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
          NewsId = newsId,
          Date = newsDate,
          Title = newsTitle,
          Text = newsText,
          Author = newsAuthor
        }
      };
    }
  }
}
