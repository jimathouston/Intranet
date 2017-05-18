using Intranet.API.Controllers;
using Intranet.API.Domain.Data;
using Intranet.API.Domain.Models.Entities;
using Intranet.API.UnitTests.Fakes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections;
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
      News newsItem = GetFakeNews().First();

      var context = DbContextFake.GetDbContext<IntranetApiContext>();

      var newsController = new NewsController(context);

      // Act
      newsController.ModelState.AddModelError(nameof(News.Author), "Author must be specified");
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
      var newsItem = GetFakeNews().First();

      var context = DbContextFake.GetDbContext<IntranetApiContext>();

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
      var newsItem = GetFakeNews().First();

      var context = DbContextFake.GetDbContext<IntranetApiContext>();

      var newsController = new NewsController(context);

      // Act
      newsController.Post(newsItem);
      var news = context.News.First();
      context.Dispose();

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
      var news = GetFakeNews();
      int id = 1;

      var context = DbContextFake.GetDbContext<IntranetApiContext>
      (
        property: nameof(IntranetApiContext.News),
        obj: news
      );

      var newsController = new NewsController(context);

      // Act
      newsController.ModelState.AddModelError(nameof(News.Title), "Title must be specified");
      var result = newsController.Put(id, news.First());
      context.Dispose();

      // Assert
      var badReqResult = Assert.IsType<BadRequestObjectResult>(result);
      Assert.IsType<SerializableError>(badReqResult.Value);
    }

    [Fact]
    public void ReturnNotFoundWhenUpdate()
    {
      // Assign
      var news = GetFakeNews();
      int id = 5;

      var context = DbContextFake.GetDbContext<IntranetApiContext>
      (
        property: nameof(IntranetApiContext.News),
        obj: news
      );

      var newsController = new NewsController(context);

      // Act
      var result = newsController.Put(id, news.First());
      context.Dispose();

      // Assert
      Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public void ReturnOkResultWhenUpdate()
    {
      // Assign
      var oldNewsItem = GetFakeNews().First();
      var newNewsItem = GetFakeNews().First();

      newNewsItem.Author = "Connie the Consultant";

      var context = DbContextFake.GetDbContext<IntranetApiContext>
      (
        property: nameof(IntranetApiContext.News),
        obj: oldNewsItem
      );

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

      var context = DbContextFake.GetDbContext<IntranetApiContext>
      (
        property: nameof(IntranetApiContext.News),
        obj: news
      );

      var newsController = new NewsController(context);

      // Act
      var result = newsController.Get(id);
      var okObjectResult = result as OkObjectResult;
      var newsContent = okObjectResult.Value as News;

      // Assert
      Assert.NotNull(okObjectResult);
      Assert.Equal(id, newsContent.Id);
    }

    [Fact]
    public void ReturnOkObjectResultWhenSearchById()
    {
      // Assign
      int id = 1;
      var news = GetFakeNews();
      var context = DbContextFake.GetDbContext<IntranetApiContext>
      (
        property: nameof(IntranetApiContext.News),
        obj: news
      );

      var newsController = new NewsController(context);

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
      var context = DbContextFake.GetDbContext<IntranetApiContext>
      (
        property: nameof(IntranetApiContext.News),
        obj: news
      );

      var newsController = new NewsController(context);

      // Act
      var result = newsController.Get(id);

      // Assert
      Assert.IsType<NotFoundResult>(result);
    }

    [Theory]
    [InlineData(1, "2017-04-03 02:00:00", "News title 1", "This is a content placeholder for news title 1.", "Anne the Admin")]
    public void ReturnCorrectNewsInfoWhenGetAllNews(int newsId, string newsDate, string newsTitle, string newsText, string newsAuthor)
    {
      // Assign
      var utcDate = new DateTimeOffset(Convert.ToDateTime(newsDate));
      var news = GetFakeNews(newsId, utcDate, newsTitle, newsText, newsAuthor);

      var context = DbContextFake.GetDbContext<IntranetApiContext>
      (
        property: nameof(IntranetApiContext.News),
        obj: news
      );

      var newsController = new NewsController(context);

      // Act
      var fetchNews = newsController.Get();
      var okObjectResult = fetchNews as OkObjectResult;
      var iEnumerable = okObjectResult.Value as IEnumerable<News>;
      var newsFromController = iEnumerable.First();

      // Assert
      Assert.Equal(newsFromController.Id, newsId);
      Assert.Equal(newsFromController.Date, utcDate);
      Assert.Equal(newsFromController.Title, newsTitle);
      Assert.Equal(newsFromController.Text, newsText);
      Assert.Equal(newsFromController.Author, newsAuthor);
    }

    [Fact]
    public void ReturnNotFoundWhenGetAllNews()
    {
      // Assign
      var context = DbContextFake.GetDbContext<IntranetApiContext>
      (
        property: nameof(IntranetApiContext.News),
        obj: new List<News>()
      );

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
      var news = GetFakeNews();
      
      var context = DbContextFake.GetDbContext<IntranetApiContext>
      (
        property: nameof(IntranetApiContext.News),
        obj: news
      );

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
          newsAuthor: "Anne the Admin"
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
