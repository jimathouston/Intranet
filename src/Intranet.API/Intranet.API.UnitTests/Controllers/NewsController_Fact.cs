using Intranet.API.Controllers;
using Intranet.API.Domain.Data;
using Intranet.API.Domain.Models.Entities;
using Intranet.API.UnitTests.Fakes;
using Intranet.API.ViewModels;
using Intranet.Shared.Factories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using Xunit;

namespace Intranet.API.UnitTests.Controllers
{
    public class NewsController_Fact
    {
        #region Post
        [Fact]
        public void ReturnOkResultWhenPosting()
        {
            // Assign
            var newsItem = GetFakeNews().First();
            var newsItemVM = new NewsViewModel(newsItem);
            var username = "test.user";
            var user = new ClaimsPrincipalFake(new Claim("username", username));
            var dateTimeFactory = new DateTimeFactory();

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var newsController = ControllerFake.GetController<NewsController>(context, dateTimeFactory);
                newsController.ControllerContext.HttpContext.User = user;

                // Act
                var result = newsController.Post(newsItemVM);

                // Assert
                Assert.IsType<OkObjectResult>(result);
            }
        }

        [Fact]
        public void CheckNewsItemWasCorrectlyPosted()
        {
            // Assign
            var newsItem = GetFakeNews().First();
            var dateTimeFactory = new DateTimeFactory();
            var username = "test.user";
            var displayName = "Test User";
            var user = new ClaimsPrincipalFake(new[] {
                new Claim("username", username),
                new Claim("displayName", displayName),
            });

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var newsController = ControllerFake.GetController<NewsController>(context, dateTimeFactory);
                newsController.ControllerContext.HttpContext.User = user;

                // Act
                newsController.Post(new NewsViewModel(newsItem));
            }

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var news = context.News.Include(m => m.User).First();

                // Assert
                Assert.NotNull(news);
                Assert.True(news.Text == newsItem.Text);
                Assert.True(news.Title == newsItem.Title);
                Assert.True(news.UserId == username);
                Assert.True(news.User.Username == username);
                Assert.True(news.User.DisplayName == displayName);
            }
        }

        [Fact]
        public void SetPublishDateWhenCreatingNews()
        {
            // Assign
            var dateTimeOffsetCreated = new DateTimeOffset(2017, 7, 18, 0, 0, 0, TimeSpan.Zero);
            var newsItem = GetFakeNews(dateTimeOffsetCreated).First();
            var dateTimeFactoryCreatedMock = new Mock<IDateTimeFactory>();

            dateTimeFactoryCreatedMock.SetupGet(d => d.DateTimeOffsetUtc).Returns(dateTimeOffsetCreated);

            var user = new ClaimsPrincipalFake(new Claim("username", "anne.the.admin"));

            newsItem.UserId = "anne.the.admin";
            newsItem.Published = true;

            DbContextFake.SeedDb<IntranetApiContext>(c => c.News.AddRange(newsItem), ensureDeleted: true);

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var newsController = ControllerFake.GetController<NewsController>(context, dateTimeFactoryCreatedMock.Object);
                newsController.ControllerContext.HttpContext.User = user;

                // Act
                var result = newsController.Post(new NewsViewModel(newsItem));
            }

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                // Assert
                Assert.Equal(context.News.First().Created, dateTimeOffsetCreated);
                Assert.Equal(context.News.First().Updated, dateTimeOffsetCreated);
                Assert.True(context.News.First().Published);
                Assert.True(context.News.First().HasEverBeenPublished);
            }
        }
        #endregion

        #region Put
        [Fact]
        public void ReturnNotFoundWhenUpdate()
        {
            // Assign
            var news = GetFakeNews();
            int id = 5;
            var dateTimeFactory = new DateTimeFactory();

            DbContextFake.SeedDb<IntranetApiContext>(c => c.News.AddRange(news), ensureDeleted: true);

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var newsController = ControllerFake.GetController<NewsController>(context, dateTimeFactory);

                // Act
                var result = newsController.Put(id, new NewsViewModel(news.First()));

                // Assert
                Assert.IsType<NotFoundObjectResult>(result);
            }
        }

        [Fact]
        public void ReturnForbidWhenUpdateOtherNews()
        {
            // Assign
            var oldNewsItem = GetFakeNews().First();
            var newNewsItem = GetFakeNews().First();
            var dateTimeFactory = new DateTimeFactory();

            newNewsItem.UserId = "connie.the.consultant";

            DbContextFake.SeedDb<IntranetApiContext>(c => c.News.AddRange(oldNewsItem), ensureDeleted: true);

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var newsController = ControllerFake.GetController<NewsController>(context, dateTimeFactory);

                // Act
                var result = newsController.Put(1, new NewsViewModel(newNewsItem));

                // Assert
                Assert.IsType<ForbidResult>(result);
            }
        }

        [Fact]
        public void ReturnOkResultWhenUpdateOwnNews()
        {
            // Assign
            var oldNewsItem = GetFakeNews().First();
            var newNewsItem = GetFakeNews().First();
            var dateTimeFactory = new DateTimeFactory();
            var user = new ClaimsPrincipalFake(new Claim("username", "anne.the.admin"));

            newNewsItem.UserId = "anne.the.admin";

            DbContextFake.SeedDb<IntranetApiContext>(c => c.News.AddRange(oldNewsItem), ensureDeleted: true);

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var newsController = ControllerFake.GetController<NewsController>(context, dateTimeFactory);
                newsController.ControllerContext.HttpContext.User = user;

                // Act
                var result = newsController.Put(1, new NewsViewModel(newNewsItem));

                // Assert
                Assert.IsType<OkObjectResult>(result);
            }
        }

        [Fact]
        public void SetPublishDateWhenUpdateNews()
        {
            // Assign
            var dateTimeOffsetCreated = new DateTimeOffset(2017, 7, 18, 0, 0, 0, TimeSpan.Zero);
            var dateTimeOffsetUpdated = new DateTimeOffset(2017, 7, 19, 0, 0, 0, TimeSpan.Zero);
            var oldNewsItem = GetFakeNews(dateTimeOffsetCreated).First();
            var newNewsItem = GetFakeNews(dateTimeOffsetCreated).First();
            var dateTimeFactoryCreatedMock = new Mock<IDateTimeFactory>();
            var dateTimeFactoryUpdatedMock = new Mock<IDateTimeFactory>();
            
            dateTimeFactoryUpdatedMock.SetupGet(d => d.DateTimeOffsetUtc).Returns(dateTimeOffsetUpdated);

            var user = new ClaimsPrincipalFake(new Claim("username", "anne.the.admin"));

            oldNewsItem.Published = true;

            newNewsItem.UserId = "anne.the.admin";
            newNewsItem.Published = true;

            DbContextFake.SeedDb<IntranetApiContext>(c => c.News.AddRange(oldNewsItem), ensureDeleted: true);

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var newsController = ControllerFake.GetController<NewsController>(context, dateTimeFactoryUpdatedMock.Object);
                newsController.ControllerContext.HttpContext.User = user;
                
                // Act
                var result = newsController.Put(oldNewsItem.Id, new NewsViewModel(newNewsItem));
            }

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                // Assert
                Assert.Equal(context.News.First().Created, dateTimeOffsetCreated);
                Assert.Equal(context.News.First().Updated, dateTimeOffsetUpdated);
            }
        }

        [Fact]
        public void ReturnOkResultWhenUpdateAsAdmin()
        {
            // Assign
            var oldNewsItem = GetFakeNews().First();
            var newNewsItem = GetFakeNews().First();
            var dateTimeFactory = new DateTimeFactory();
            var user = new ClaimsPrincipalFake(new Claim("role", "admin"));

            newNewsItem.UserId = "anne.the.admin";

            DbContextFake.SeedDb<IntranetApiContext>(c => c.News.AddRange(oldNewsItem), ensureDeleted: true);

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var newsController = ControllerFake.GetController<NewsController>(context, dateTimeFactory);
                newsController.ControllerContext.HttpContext.User = user;

                // Act
                var result = newsController.Put(1, new NewsViewModel(newNewsItem));

                // Assert
                Assert.IsType<OkObjectResult>(result);
            }
        }
        #endregion

        #region Delete
        [Fact]
        public void ReturnNotFoundWhenDelete()
        {
            // Assign
            var news = GetFakeNews();
            int id = 5;
            var dateTimeFactory = new DateTimeFactory();

            DbContextFake.SeedDb<IntranetApiContext>(c => c.News.AddRange(news), ensureDeleted: true);

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var newsController = ControllerFake.GetController<NewsController>(context, dateTimeFactory);

                // Act
                var result = newsController.Delete(id);

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public void ReturnForbidWhenDeleteOtherNews()
        {
            // Assign
            var news = GetFakeNews().First();
            var dateTimeFactory = new DateTimeFactory();

            DbContextFake.SeedDb<IntranetApiContext>(c => c.News.AddRange(news), ensureDeleted: true);

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var newsController = ControllerFake.GetController<NewsController>(context, dateTimeFactory);

                // Act
                var result = newsController.Delete(1);

                // Assert
                Assert.IsType<ForbidResult>(result);
            }
        }

        [Fact]
        public void ReturnOkResultWhenDeleteOwnNews()
        {
            // Assign
            var news = GetFakeNews().First();
            var user = new ClaimsPrincipalFake(new Claim("username", "anne.the.admin"));
            var dateTimeFactory = new DateTimeFactory();

            DbContextFake.SeedDb<IntranetApiContext>(c => c.News.AddRange(news), ensureDeleted: true);

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var newsController = ControllerFake.GetController<NewsController>(context, dateTimeFactory);
                newsController.ControllerContext.HttpContext.User = user;

                // Act
                var result = newsController.Delete(1);

                // Assert
                Assert.IsType<OkResult>(result);
            }
        }

        [Fact]
        public void ReturnOkResultWhenDeleteAsAdmin()
        {
            // Assign
            var news = GetFakeNews().First();
            var user = new ClaimsPrincipalFake(new Claim("role", "admin"));
            var dateTimeFactory = new DateTimeFactory();

            DbContextFake.SeedDb<IntranetApiContext>(c => c.News.AddRange(news), ensureDeleted: true);

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var newsController = ControllerFake.GetController<NewsController>(context, dateTimeFactory);
                newsController.ControllerContext.HttpContext.User = user;

                // Act
                var result = newsController.Delete(1);

                // Assert
                Assert.IsType<OkResult>(result);
            }
        }
        #endregion

        #region Get
        [Fact]
        public void ReturnNewsById()
        {
            // Assign
            var id = 1;
            var news = GetFakeNews();
            var dateTimeFactory = new DateTimeFactory();

            DbContextFake.SeedDb<IntranetApiContext>(c => c.News.AddRange(news), ensureDeleted: true);

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var newsController = new NewsController(context, dateTimeFactory);

                // Act
                var result = newsController.Get(id);
                var okObjectResult = result as OkObjectResult;
                var newsContent = okObjectResult.Value as News;

                // Assert
                Assert.NotNull(okObjectResult);
                Assert.Equal(id, newsContent.Id);
            }
        }

        [Fact]
        public void ReturnOkObjectResultWhenSearchById()
        {
            // Assign
            int id = 1;
            var news = GetFakeNews();
            var dateTimeFactory = new DateTimeFactory();

            DbContextFake.SeedDb<IntranetApiContext>(c => c.News.AddRange(news), ensureDeleted: true);

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var newsController = new NewsController(context, dateTimeFactory);

                // Act
                var result = newsController.Get(id);

                // Assert
                Assert.IsType<OkObjectResult>(result);
            }
        }

        [Fact]
        public void ReturnNotFoundResultWhenSearchById()
        {
            // Assign
            int id = 2;
            var news = GetFakeNews();
            var dateTimeFactory = new DateTimeFactory();

            DbContextFake.SeedDb<IntranetApiContext>(c => c.News.AddRange(news), ensureDeleted: true);

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var newsController = new NewsController(context, dateTimeFactory);

                // Act
                var result = newsController.Get(id);

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Theory]
        [InlineData(1, "2017-04-03 02:00:00", "News title 1", "This is a content placeholder for news title 1.", "anne.the.admin")]
        public void ReturnCorrectNewsInfoWhenGetAllNews(int id, string newsDate, string title, string text, string username)
        {
            // Assign
            var utcDate = new DateTimeOffset(Convert.ToDateTime(newsDate));
            var news = GetFakeNews(id, utcDate, title, text, username);
            var dateTimeFactory = new DateTimeFactory();

            DbContextFake.SeedDb<IntranetApiContext>(c => c.News.AddRange(news), ensureDeleted: true);

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var newsController = new NewsController(context, dateTimeFactory);

                // Act
                var fetchNews = newsController.Get();
                var okObjectResult = fetchNews as OkObjectResult;
                var iEnumerable = okObjectResult.Value as IEnumerable<News>;
                var newsFromController = iEnumerable.First();

                // Assert
                Assert.Equal(newsFromController.Id, id);
                Assert.Equal(newsFromController.Created, utcDate);
                Assert.Equal(newsFromController.Title, title);
                Assert.Equal(newsFromController.Text, text);
                Assert.Equal(newsFromController.UserId, username);
            }
        }

        [Fact]
        public void ReturnOkWhenGetAllNewsIsEmpty()
        {
            // Assign
            var dateTimeFactory = new DateTimeFactory();

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var newsController = new NewsController(context, dateTimeFactory);

                // Act
                var newsFromController = newsController.Get();
                var count = GetCountOfValueProperty(newsFromController);

                // Assert
                Assert.IsType<OkObjectResult>(newsFromController);
                Assert.Equal(count, 0);
            }
        }

        [Fact]
        public void ReturnOkObjectResultWhenGetAllNews()
        {
            // Assign
            var news = GetFakeNews();
            var dateTimeFactory = new DateTimeFactory();

            DbContextFake.SeedDb<IntranetApiContext>(c => c.News.AddRange(news), ensureDeleted: true);

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var newsController = new NewsController(context, dateTimeFactory);

                // Act
                var newsFromController = newsController.Get();
                var count = GetCountOfValueProperty(newsFromController);

                // Assert
                Assert.IsType<OkObjectResult>(newsFromController);
                Assert.Equal(count, 1);
            }
        }
        #endregion

        #region Private Helpers
        private static int GetCountOfValueProperty(IActionResult newsFromController)
        {
            var ValuePropValue = newsFromController.GetType().GetProperty("Value").GetValue(newsFromController);
            var CountPropValue = (int)ValuePropValue.GetType().GetProperty("Count").GetValue(ValuePropValue);
            return CountPropValue;
        }

        private IEnumerable<News> GetFakeNews()
        {
            return GetFakeNews(newsDate: DateTimeOffset.Now);
        }

        private IEnumerable<News> GetFakeNews(DateTimeOffset newsDate)
        {
            return GetFakeNews
                (
                    id: 1,
                    date: newsDate,
                    title: "News title 1",
                    text: "This is a content placeholder for news title 1",
                    username: "anne.the.admin"
                );
        }

        /// <summary>
        /// Generate and return dummy news 
        /// </summary>
        /// <returns></returns>
        private IEnumerable<News> GetFakeNews(int id,
                                              DateTimeOffset date,
                                              string title,
                                              string text,
                                              string username)
        {
            return new News[]
            {
                new News
                {
                    Id = id,
                    Created = date,
                    Title = title,
                    Text = text,
                    UserId = username
                }
            };
        }
        #endregion
    }
}
