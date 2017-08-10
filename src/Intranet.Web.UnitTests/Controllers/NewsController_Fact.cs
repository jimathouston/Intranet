using Intranet.Web.Controllers;
using Intranet.Web.Domain.Data;
using Intranet.Web.Domain.Models.Entities;
using Intranet.Test.Tools.Fakes;
using Intranet.Test.Tools.Extensions;
using Intranet.Web.ViewModels;
using Intranet.Web.Common.Factories;
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
using System.Threading.Tasks;
using Intranet.Services.FileStorageService;
using X.PagedList;

namespace Intranet.Web.UnitTests.Controllers
{
    public class NewsController_Fact
    {
        #region CREATE
        [Fact]
        public void ValidModelStateOnGet()
        {
            // Assign
            var news = GetFakeNews();
            var dateTimeFactory = new DateTimeFactory();
            var fileServiceMock = new Mock<IFileStorageService>();

            DbContextFake.SeedDb<IntranetApiContext>(c => c.News.AddRange(news), ensureDeleted: true);

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var newsController = new NewsController(context, dateTimeFactory, fileServiceMock.Object);

                // Act
                var response = newsController.Create();

                // Assert
                Assert.IsType<ViewResult>(response);
            }
        }

        [Fact]
        public async Task ValidModelStateWhenPosting()
        {
            // Assign
            var newsItem = GetFakeNews().First();
            var newsItemVM = new NewsViewModel(newsItem);
            var username = "test.user";
            var user = new ClaimsPrincipalFake(new Claim(ClaimTypes.NameIdentifier, username));
            var dateTimeFactory = new DateTimeFactory();
            var fileServiceMock = new Mock<IFileStorageService>();

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var newsController = ControllerFake.GetController<NewsController>(context, dateTimeFactory, fileServiceMock.Object);
                newsController.ControllerContext.HttpContext.User = user;

                // Act
                var result = await newsController.Create(newsItemVM);

                // Assert
                Assert.True(result.ValidateActionRedirect("Details"));
            }
        }

        [Fact]
        public async Task ReturnInvalidModelStateWhenPostingDuplicate()
        {
            // Assign
            var newsItem = GetFakeNews().First();
            var newsItemVM = new NewsViewModel(newsItem);
            var username = "test.user";
            var user = new ClaimsPrincipalFake(new Claim(ClaimTypes.NameIdentifier, username));
            var dateTimeFactory = new DateTimeFactory();
            var fileServiceMock = new Mock<IFileStorageService>();

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var newsController = ControllerFake.GetController<NewsController>(context, dateTimeFactory, fileServiceMock.Object);
                newsController.ControllerContext.HttpContext.User = user;

                // Act
                await newsController.Create(newsItemVM);
                var result = await newsController.Create(newsItemVM);

                // Assert
                Assert.False(result.ModelStateIsValid());
            }
        }

        [Fact]
        public async Task CheckNewsItemWasCorrectlyPosted()
        {
            // Assign
            var newsItem = GetFakeNews().First();
            var dateTimeFactory = new DateTimeFactory();
            var fileServiceMock = new Mock<IFileStorageService>();
            var username = "test.user";
            var displayName = "Test User";
            var user = new ClaimsPrincipalFake(new[] {
                new Claim(ClaimTypes.NameIdentifier, username),
                new Claim(ClaimTypes.Name, displayName),
            });

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var newsController = ControllerFake.GetController<NewsController>(context, dateTimeFactory, fileServiceMock.Object);
                newsController.ControllerContext.HttpContext.User = user;

                // Act
                await newsController.Create(new NewsViewModel(newsItem));
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
        public async Task SetPublishDateWhenCreatingNews()
        {
            // Assign
            var dateTimeOffsetCreated = new DateTimeOffset(2017, 7, 18, 0, 0, 0, TimeSpan.Zero);
            var newsItem = GetFakeNews(dateTimeOffsetCreated).First();
            var dateTimeFactoryCreatedMock = new Mock<IDateTimeFactory>();
            var fileServiceMock = new Mock<IFileStorageService>();

            dateTimeFactoryCreatedMock.SetupGet(d => d.DateTimeOffsetUtc).Returns(dateTimeOffsetCreated);

            var user = new ClaimsPrincipalFake(new Claim(ClaimTypes.NameIdentifier, "anne.the.admin"));

            newsItem.UserId = "anne.the.admin";
            newsItem.Published = true;

            DbContextFake.SeedDb<IntranetApiContext>(c => c.News.AddRange(newsItem), ensureDeleted: true);

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var newsController = ControllerFake.GetController<NewsController>(context, dateTimeFactoryCreatedMock.Object, fileServiceMock.Object);
                newsController.ControllerContext.HttpContext.User = user;

                // Act
                var result = await newsController.Create(new NewsViewModel(newsItem));
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

        #region UPDATE
        [Fact]
        public async Task ValidModelStateOnGetBeforeUpdate()
        {
            // Assign
            var id = 1;
            var news = GetFakeNews();
            var dateTimeFactory = new DateTimeFactory();
            var fileServiceMock = new Mock<IFileStorageService>();

            DbContextFake.SeedDb<IntranetApiContext>(c => c.News.AddRange(news), ensureDeleted: true);

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var newsController = new NewsController(context, dateTimeFactory, fileServiceMock.Object);

                // Act
                var response = await newsController.Edit(id);
                var newsContent = response.GetModelAs<NewsViewModel>();

                // Assert
                Assert.IsType<ViewResult>(response);
                Assert.Equal(id, newsContent.Id);
            }
        }

        [Fact]
        public async Task ReturnNotFoundWhenUpdate()
        {
            // Assign
            var news = GetFakeNews();
            int id = 5;
            var dateTimeFactory = new DateTimeFactory();
            var fileServiceMock = new Mock<IFileStorageService>();

            DbContextFake.SeedDb<IntranetApiContext>(c => c.News.AddRange(news), ensureDeleted: true);

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var newsController = ControllerFake.GetController<NewsController>(context, dateTimeFactory, fileServiceMock.Object);

                // Act
                var result = await newsController.Edit(id, new NewsViewModel(news.First()));

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task ReturnInvalidModelStateWhenUpdateOtherNews()
        {
            // Assign
            var oldNewsItem = GetFakeNews().First();
            var newNewsItem = GetFakeNews().First();
            var dateTimeFactory = new DateTimeFactory();
            var fileServiceMock = new Mock<IFileStorageService>();

            newNewsItem.UserId = "connie.the.consultant";

            DbContextFake.SeedDb<IntranetApiContext>(c => c.News.AddRange(oldNewsItem), ensureDeleted: true);

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var newsController = ControllerFake.GetController<NewsController>(context, dateTimeFactory, fileServiceMock.Object);

                // Act
                var result = await newsController.Edit(1, new NewsViewModel(newNewsItem));

                // Assert
                Assert.False(result.ModelStateIsValid());
            }
        }

        [Fact]
        public async Task RedirectWhenUpdateOwnNews()
        {
            // Assign
            var oldNewsItem = GetFakeNews().First();
            var newNewsItem = GetFakeNews().First();
            var dateTimeFactory = new DateTimeFactory();
            var fileServiceMock = new Mock<IFileStorageService>();
            var user = new ClaimsPrincipalFake(new Claim(ClaimTypes.NameIdentifier, "anne.the.admin"));

            newNewsItem.UserId = "anne.the.admin";

            DbContextFake.SeedDb<IntranetApiContext>(c => c.News.AddRange(oldNewsItem), ensureDeleted: true);

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var newsController = ControllerFake.GetController<NewsController>(context, dateTimeFactory, fileServiceMock.Object);
                newsController.ControllerContext.HttpContext.User = user;

                // Act
                var result = await newsController.Edit(1, new NewsViewModel(newNewsItem));

                // Assert
                Assert.True(result.ValidateActionRedirect("Details"));
            }
        }

        [Fact]
        public async Task SetPublishDateWhenUpdateNews()
        {
            // Assign
            var dateTimeOffsetCreated = new DateTimeOffset(2017, 7, 18, 0, 0, 0, TimeSpan.Zero);
            var dateTimeOffsetUpdated = new DateTimeOffset(2017, 7, 19, 0, 0, 0, TimeSpan.Zero);
            var oldNewsItem = GetFakeNews(dateTimeOffsetCreated).First();
            var newNewsItem = GetFakeNews(dateTimeOffsetCreated).First();
            var dateTimeFactoryCreatedMock = new Mock<IDateTimeFactory>();
            var dateTimeFactoryUpdatedMock = new Mock<IDateTimeFactory>();
            var fileServiceMock = new Mock<IFileStorageService>();

            dateTimeFactoryUpdatedMock.SetupGet(d => d.DateTimeOffsetUtc).Returns(dateTimeOffsetUpdated);

            var user = new ClaimsPrincipalFake(new Claim(ClaimTypes.NameIdentifier, "anne.the.admin"));

            oldNewsItem.Published = true;

            newNewsItem.UserId = "anne.the.admin";
            newNewsItem.Published = true;

            DbContextFake.SeedDb<IntranetApiContext>(c => c.News.AddRange(oldNewsItem), ensureDeleted: true);

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var newsController = ControllerFake.GetController<NewsController>(context, dateTimeFactoryUpdatedMock.Object, fileServiceMock.Object);
                newsController.ControllerContext.HttpContext.User = user;
                
                // Act
                var result = await newsController.Edit(oldNewsItem.Id, new NewsViewModel(newNewsItem));
            }

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                // Assert
                Assert.Equal(context.News.First().Created, dateTimeOffsetCreated);
                Assert.Equal(context.News.First().Updated, dateTimeOffsetUpdated);
            }
        }

        [Fact]
        public async Task RedirectWhenUpdateAsAdmin()
        {
            // Assign
            var oldNewsItem = GetFakeNews().First();
            var newNewsItem = GetFakeNews().First();
            var dateTimeFactory = new DateTimeFactory();
            var fileServiceMock = new Mock<IFileStorageService>();
            var user = new ClaimsPrincipalFake(new Claim(ClaimTypes.Role, "Admin"));

            newNewsItem.UserId = "anne.the.admin";

            DbContextFake.SeedDb<IntranetApiContext>(c => c.News.AddRange(oldNewsItem), ensureDeleted: true);

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var newsController = ControllerFake.GetController<NewsController>(context, dateTimeFactory, fileServiceMock.Object);
                newsController.ControllerContext.HttpContext.User = user;

                // Act
                var result = await newsController.Edit(1, new NewsViewModel(newNewsItem));

                // Assert
                Assert.True(result.ValidateActionRedirect("Details"));
            }
        }
        #endregion

        #region DELETE
        [Fact]
        public async Task ValidModelStateOnGetBeforeDelete()
        {
            // Assign
            var id = 1;
            var news = GetFakeNews();
            var dateTimeFactory = new DateTimeFactory();
            var fileServiceMock = new Mock<IFileStorageService>();

            DbContextFake.SeedDb<IntranetApiContext>(c => c.News.AddRange(news), ensureDeleted: true);

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var newsController = new NewsController(context, dateTimeFactory, fileServiceMock.Object);

                // Act
                var response = await newsController.Delete(id);
                var newsContent = response.GetModelAs<NewsViewModel>();

                // Assert
                Assert.IsType<ViewResult>(response);
                Assert.Equal(id, newsContent.Id);
            }
        }

        [Fact]
        public async Task ReturnNotFoundWhenDelete()
        {
            // Assign
            var news = GetFakeNews();
            int id = 5;
            var dateTimeFactory = new DateTimeFactory();
            var fileServiceMock = new Mock<IFileStorageService>();

            DbContextFake.SeedDb<IntranetApiContext>(c => c.News.AddRange(news), ensureDeleted: true);

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var newsController = ControllerFake.GetController<NewsController>(context, dateTimeFactory, fileServiceMock.Object);

                // Act
                var result = await newsController.Delete(id);

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task ReturnInvalidModelStateWhenDeleteOtherNews()
        {
            // Assign
            var news = GetFakeNews().First();
            var dateTimeFactory = new DateTimeFactory();
            var fileServiceMock = new Mock<IFileStorageService>();

            DbContextFake.SeedDb<IntranetApiContext>(c => c.News.AddRange(news), ensureDeleted: true);

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var newsController = ControllerFake.GetController<NewsController>(context, dateTimeFactory, fileServiceMock.Object);

                // Act
                var result = await newsController.DeleteConfirmed(1);

                // Assert
                Assert.False(result.ModelStateIsValid());
            }
        }

        [Fact]
        public async Task ValidModelStateWhenDeleteOwnNews()
        {
            // Assign
            var news = GetFakeNews().First();
            var user = new ClaimsPrincipalFake(new Claim(ClaimTypes.NameIdentifier, "anne.the.admin"));
            var dateTimeFactory = new DateTimeFactory();
            var fileServiceMock = new Mock<IFileStorageService>();

            DbContextFake.SeedDb<IntranetApiContext>(c => c.News.AddRange(news), ensureDeleted: true);

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var newsController = ControllerFake.GetController<NewsController>(context, dateTimeFactory, fileServiceMock.Object);
                newsController.ControllerContext.HttpContext.User = user;

                // Act
                var result = await newsController.Delete(1);

                // Assert
                Assert.IsType<ViewResult>(result);
                Assert.True(result.ModelStateIsValid());
            }
        }

        [Fact]
        public async Task RedirectWhenDeleteAsAdmin()
        {
            // Assign
            var news = GetFakeNews().First();
            var user = new ClaimsPrincipalFake(new Claim(ClaimTypes.Role, "Admin"));
            var dateTimeFactory = new DateTimeFactory();
            var fileServiceMock = new Mock<IFileStorageService>();

            DbContextFake.SeedDb<IntranetApiContext>(c => c.News.AddRange(news), ensureDeleted: true);

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var newsController = ControllerFake.GetController<NewsController>(context, dateTimeFactory, fileServiceMock.Object);
                newsController.ControllerContext.HttpContext.User = user;

                // Act
                var result = await newsController.DeleteConfirmed(1);

                // Assert
                Assert.True(result.ValidateActionRedirect("Index"));
            }
        }
        #endregion

        #region GET
        [Fact]
        public async Task Get_News_Id_Should_Return_News()
        {
            // Assign
            var id = 1;
            var news = GetFakeNews();
            var dateTimeFactory = new DateTimeFactory();
            var fileServiceMock = new Mock<IFileStorageService>();

            DbContextFake.SeedDb<IntranetApiContext>(c => c.News.AddRange(news), ensureDeleted: true);

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var newsController = new NewsController(context, dateTimeFactory, fileServiceMock.Object);

                // Act
                var response = await newsController.Details(id);
                var newsContent = response.GetModelAs<NewsViewModel>();

                // Assert
                Assert.IsType<ViewResult>(response);
                Assert.Equal(id, newsContent.Id);
            }
        }

        [Fact]
        public async Task ReturnOkObjectResultWhenSearchById()
        {
            // Assign
            int id = 1;
            var news = GetFakeNews();
            var dateTimeFactory = new DateTimeFactory();
            var fileServiceMock = new Mock<IFileStorageService>();

            DbContextFake.SeedDb<IntranetApiContext>(c => c.News.AddRange(news), ensureDeleted: true);

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var newsController = new NewsController(context, dateTimeFactory, fileServiceMock.Object);

                // Act
                var response = await newsController.Details(id);
                var result = response.GetModelAs<NewsViewModel>();

                // Assert
                Assert.IsType<ViewResult>(response);
                Assert.Equal(1, result.Id);
            }
        }

        [Fact]
        public async Task ReturnNotFoundResultWhenSearchById()
        {
            // Assign
            int id = 2;
            var news = GetFakeNews();
            var dateTimeFactory = new DateTimeFactory();
            var fileServiceMock = new Mock<IFileStorageService>();

            DbContextFake.SeedDb<IntranetApiContext>(c => c.News.AddRange(news), ensureDeleted: true);

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var newsController = new NewsController(context, dateTimeFactory, fileServiceMock.Object);

                // Act
                var result = await newsController.Details(id);

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task ReturnNewsByUrl()
        {
            // Assign
            var url = "news-title-1";
            var news = GetFakeNews();
            var dateTimeFactory = new DateTimeFactory();
            var fileServiceMock = new Mock<IFileStorageService>();

            var newsToFind = news.SingleOrDefault(n => n.Url == url);

            DbContextFake.SeedDb<IntranetApiContext>(c => c.News.AddRange(news), ensureDeleted: true);

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var newsController = new NewsController(context, dateTimeFactory, fileServiceMock.Object);

                // Act
                var response = await newsController.Details(newsToFind.Created.Year, newsToFind.Created.Month, newsToFind.Created.Day, url);
                var newsContent = response.GetModelAs<NewsViewModel>();

                // Assert
                Assert.Equal(url, newsContent.Url);
                Assert.IsType<ViewResult>(response);
            }
        }

        [Fact]
        public async Task ReturnNotFoundResultWhenSearchByUrl()
        {
            // Assign
            var url = "news-title-2";
            var news = GetFakeNews();
            var dateTimeFactory = new DateTimeFactory();
            var fileServiceMock = new Mock<IFileStorageService>();

            DbContextFake.SeedDb<IntranetApiContext>(c => c.News.AddRange(news), ensureDeleted: true);

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var newsController = new NewsController(context, dateTimeFactory, fileServiceMock.Object);

                // Act
                var result = await newsController.Details(2017, 7, 21, url);

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task ReturnNotFoundResultWhenSearchByWrongDateUrl()
        {
            // Assign
            var dateTimeOffsetCreated = new DateTimeOffset(2017, 7, 18, 0, 0, 0, TimeSpan.Zero);
            var url = "news-title-1";
            var news = GetFakeNews(dateTimeOffsetCreated);
            var dateTimeFactory = new DateTimeFactory();
            var fileServiceMock = new Mock<IFileStorageService>();

            DbContextFake.SeedDb<IntranetApiContext>(c => c.News.AddRange(news), ensureDeleted: true);

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var newsController = new NewsController(context, dateTimeFactory, fileServiceMock.Object);

                // Act
                var result = await newsController.Details(2017, 7, 21, url);

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Theory]
        [InlineData(1, "2017-04-03 02:00:00", "News title 1", "This is a content placeholder for news title 1.", "anne.the.admin", "news-title-1")]
        public async Task Get_All_News_Should_Return_All_News(int id, string newsDate, string title, string text, string username, string url)
        {
            // Assign
            var utcDate = new DateTimeOffset(Convert.ToDateTime(newsDate));
            var news = GetFakeNews(id, utcDate, title, text, username, url);
            var dateTimeFactory = new DateTimeFactory();
            var fileServiceMock = new Mock<IFileStorageService>();

            DbContextFake.SeedDb<IntranetApiContext>(c => c.News.AddRange(news), ensureDeleted: true);

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var newsController = new NewsController(context, dateTimeFactory, fileServiceMock.Object);

                // Act
                var response = await newsController.Index();
                var models = response.GetModelAs<IEnumerable<NewsViewModel>>();
                var newsFromController = models.First();

                // Assert
                Assert.Equal(newsFromController.Id, id);
                Assert.Equal(newsFromController.Created, utcDate);
                Assert.Equal(newsFromController.Title, title);
                Assert.Equal(newsFromController.Text, text);
                Assert.Equal(newsFromController.UserId, username);
            }
        }

        [Fact]
        public async Task Get_All_News_Should_Return_Paginated_News()
        {
            // Assign
            var news = GetFakeNews();
            var dateTimeFactory = new DateTimeFactory();
            var fileServiceMock = new Mock<IFileStorageService>();

            DbContextFake.SeedDb<IntranetApiContext>(c => c.News.AddRange(news));

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var newsController = new NewsController(context, dateTimeFactory, fileServiceMock.Object);
                var newsController2 = new NewsController(context, dateTimeFactory, fileServiceMock.Object);

                // Act
                var firstPageResponse = await newsController.Index(1);
                var secondPageResponse = await newsController2.Index(2);
                var firstPageModels = firstPageResponse.GetModelAs<IPagedList<NewsViewModel>>();
                var secondPageModels = secondPageResponse.GetModelAs<IPagedList<NewsViewModel>>();


                // Assert
                Assert.Equal(1, firstPageModels.Count);
                Assert.Equal(0, secondPageModels.Count);
            }
        }

        [Fact]
        public async Task Get_All_News_Should_Return_Empty_List()
        {
            // Assign
            var dateTimeFactory = new DateTimeFactory();
            var fileServiceMock = new Mock<IFileStorageService>();

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var newsController = new NewsController(context, dateTimeFactory, fileServiceMock.Object);

                // Act
                var response = await newsController.Index();
                var result = response.GetModelAs<IEnumerable<News>>();

                // Assert
                Assert.IsType<ViewResult>(response);
                Assert.Equal(result.Count(), 0);
            }
        }

        [Fact]
        public async Task ReturnOkObjectResultWhenGetAllNews()
        {
            // Assign
            var news = GetFakeNews();
            var dateTimeFactory = new DateTimeFactory();
            var fileServiceMock = new Mock<IFileStorageService>();

            DbContextFake.SeedDb<IntranetApiContext>(c => c.News.AddRange(news), ensureDeleted: true);

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var newsController = new NewsController(context, dateTimeFactory, fileServiceMock.Object);

                // Act
                var newsFromController = await newsController.Index();
                var count = newsFromController.GetModelAs<IEnumerable<NewsViewModel>>().Count();

                // Assert
                Assert.IsType<ViewResult>(newsFromController);
                Assert.Equal(count, 1);
            }
        }
        #endregion

        #region Private Helpers
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
                    username: "anne.the.admin",
                    url: "news-title-1"
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
                                              string username,
                                              string url)
        {
            return new News[]
            {
                new News
                {
                    Id = id,
                    Created = date,
                    Title = title,
                    Text = text,
                    UserId = username,
                    Url = url,
                }
            };
        }
        #endregion
    }
}
