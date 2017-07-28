using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Intranet.API.Domain;
using Intranet.API.Domain.Data;
using Intranet.API.Domain.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Intranet.API.Extensions;
using Intranet.Shared.Factories;
using Intranet.API.Helpers;
using Intranet.API.ViewModels;

namespace Intranet.API.Controllers
{
    /// <summary>
    /// Manage news items.
    /// </summary>
    [Produces("application/json")]
    [Route("/api/v1/[controller]")]
    public class NewsController : Controller, IRestController<NewsViewModel>
    {
        private readonly IntranetApiContext _intranetApiContext;
        private readonly IDateTimeFactory _dateTimeFactory;

        public NewsController(IntranetApiContext intranetApiContext, IDateTimeFactory dateTimeFactory)
        {
            _dateTimeFactory = dateTimeFactory;
            _intranetApiContext = intranetApiContext;
        }

        #region POST
        /// <summary>
        /// Add a new news item.
        /// </summary>
        /// <param name="news"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post([FromBody] NewsViewModel news)
        {
            try
            {
                var username = HttpContext.User.GetUsername();
                var displayName = HttpContext.User.GetDisplayName();

                if (_intranetApiContext.Users.Find(username) == null)
                {
                    var user = new User
                    {
                        Username = username,
                        DisplayName = displayName,
                    };

                    _intranetApiContext.Users.Add(user);
                }

                var newNews = new News
                {
                    Title = news.Title,
                    Text = news.Text,
                    UserId = username,
                    Published = news.Published,
                    Url = UrlHelper.URLFriendly(news.Title),
                };

                if (!String.IsNullOrWhiteSpace(news.HeaderImage?.FileName))
                {
                    newNews.HeaderImage = new Image
                    {
                        FileName = news.HeaderImage.FileName
                    };
                }

                newNews.Created = _dateTimeFactory.DateTimeOffsetUtc;

                var isTitleAndDateUniqe = !_intranetApiContext.News.Any(n => n.Created.Date == newNews.Created.Date && n.Url == newNews.Url);

                if (!isTitleAndDateUniqe)
                {
                    return BadRequest(Json(new { error = "There has already been created a news with that title today!" }));
                }

                var keywords = KeywordHelper.GetKeywordsFromString(news.Keywords);
                var allKeywordEntities = GetAllKeywordEntitiesInternal(news, keywords);
                KeywordHelper.SetKeywords<News, NewsKeyword>(keywords, newNews, allKeywordEntities);

                _intranetApiContext.News.Add(newNews);
                _intranetApiContext.SaveChanges();

                var newsViewModel = new NewsViewModel(newNews);

                return Ok(newsViewModel);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        #endregion

        #region PUT
        /// <summary>
        /// Change content of a news item.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="news"></param>
        /// <returns></returns>
        [Route("{id:int}")]
        [HttpPut]
        public IActionResult Put(int id, [FromBody] NewsViewModel news)
        {
            try
            {
                var username = HttpContext.User.GetUsername();
                var isAdmin = HttpContext.User.IsAdmin();

                var contextEntity = _intranetApiContext.News
                    .Include(n => n.User)
                    .SingleOrDefault(n => n.Id == id);

                if (contextEntity == null)
                {
                    news.Id = id;
                    return NotFound(news);
                }

                if (contextEntity.UserId?.Equals(username) != true && !isAdmin)
                {
                    return Forbid();
                }

                // If the news changes to Published for the first time, set creation date
                if (!contextEntity.HasEverBeenPublished && news.Published)
                {
                    contextEntity.Created = _dateTimeFactory.DateTimeOffsetUtc;
                }
                else if (news.Published)
                {
                    contextEntity.Updated = _dateTimeFactory.DateTimeOffsetUtc;
                }

                contextEntity.Title = news.Title;
                contextEntity.Text = news.Text;
                contextEntity.Published = news.Published;

                if (contextEntity.HeaderImage != null)
                {
                    contextEntity.HeaderImage.FileName = news.HeaderImage?.FileName;
                }

                var keywords = KeywordHelper.GetKeywordsFromString(news.Keywords);
                var allKeywordEntities = GetAllKeywordEntitiesInternal(news, keywords);
                KeywordHelper.SetKeywords<News, NewsKeyword>(keywords, contextEntity, allKeywordEntities);

                _intranetApiContext.SaveChanges();

                var newsViewModel = new NewsViewModel(contextEntity);

                return Ok(newsViewModel);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        #endregion

        #region DELETE
        /// <summary>
        /// Remove a news item.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{id:int}")]
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            try
            {
                var username = HttpContext.User.GetUsername();
                var isAdmin = HttpContext.User.IsAdmin();

                var contextEntity = _intranetApiContext.News.Find(id);

                if (contextEntity == null)
                {
                    return NotFound();
                }

                if (contextEntity.UserId?.Equals(username) != true && !isAdmin)
                {
                    return Forbid();
                }

                _intranetApiContext.Remove(contextEntity);
                _intranetApiContext.SaveChanges();

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        #endregion

        #region GET
        /// <summary>
        /// Retrieve a list of all news items.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                if (!_intranetApiContext.News.Any()) return new OkObjectResult(new List<News>());

                var news = _intranetApiContext.News
                    .Include(n => n.HeaderImage)
                    .Include(n => n.User)
                    .Include(n => n.NewsKeywords)
                        .ThenInclude(nk => nk.Keyword)
                    .ToList();

                var newsViewModel = news
                    .Select(n => new NewsViewModel(n))
                    .ToList();

                return new OkObjectResult(newsViewModel);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Retrieve a specific news item.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{id:int}")]
        [HttpGet]
        public IActionResult Get(int id)
        {
            try
            {
                var news = _intranetApiContext.News
                    .Include(n => n.HeaderImage)
                    .Include(n => n.User)
                    .Include(n => n.NewsKeywords)
                        .ThenInclude(nk => nk.Keyword)
                    .SingleOrDefault(n => n.Id == id);

                if (news == null)
                {
                    return NotFound();
                }

                var newsViewModel = new NewsViewModel(news);

                return new OkObjectResult(newsViewModel);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Retrieve a specific news item.
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        [Route("{year:int}/{month:int}/{day:int}/{url}")]
        [HttpGet]
        public IActionResult Get(int year, int month, int day, string url)
        {
            try
            {
                var date = new DateTimeOffset(year, month, day, 0, 0, 0, TimeSpan.Zero);

                var news = _intranetApiContext.News
                    .Include(n => n.HeaderImage)
                    .Include(n => n.User)
                    .Include(n => n.NewsKeywords)
                        .ThenInclude(nk => nk.Keyword)
                    .SingleOrDefault(n => n.Created.Date == date.Date && n.Url == url);

                if (news == null)
                {
                    return NotFound();
                }

                var newsViewModel = new NewsViewModel(news);

                return new OkObjectResult(newsViewModel);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        #endregion

        #region Private Helpers
        private List<Keyword> GetAllKeywordEntitiesInternal(NewsViewModel news, IEnumerable<string> keywords)
        {
            return _intranetApiContext.Keywords?
            .Include(k => k.NewsKeywords)
                .ThenInclude(nk => nk.News)?
            .Where(k => keywords.Contains(k.Name, StringComparer.OrdinalIgnoreCase) || k.NewsKeywords.Any(nk => nk.NewsId.Equals(news.Id)))
            .ToList();
        }
        #endregion
    }
}
