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

namespace Intranet.API.Controllers
{
    /// <summary>
    /// Manage news items.
    /// </summary>
    [Produces("application/json")]
    [Route("/api/v1/[controller]")]
    public class NewsController : Controller, IRestController<News>
    {
        private readonly IntranetApiContext _intranetApiContext;

        public NewsController(IntranetApiContext intranetApiContext)
        {
            _intranetApiContext = intranetApiContext;
        }

        /// <summary>
        /// Add a new news item.
        /// </summary>
        /// <param name="news"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post([FromBody] News news)
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
                };

                if (!String.IsNullOrWhiteSpace(news.HeaderImage?.FileName))
                {
                    newNews.HeaderImage = new Image
                    {
                        FileName = news.HeaderImage.FileName
                    };
                }

                _intranetApiContext.News.Add(newNews);
                _intranetApiContext.SaveChanges();

                return Ok(ModelState);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Change content of a news item.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="news"></param>
        /// <returns></returns>
        [Route("{id:int}")]
        [HttpPut]
        public IActionResult Put(int id, [FromBody] News news)
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

                contextEntity.Title = news.Title;
                contextEntity.Text = news.Text;
                contextEntity.Date = DateTimeOffset.UtcNow;

                if (contextEntity.HeaderImage != null)
                {
                    contextEntity.HeaderImage.FileName = news.HeaderImage?.FileName;
                }

                _intranetApiContext.SaveChanges();
                return Ok(ModelState);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

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

        /// <summary>
        /// Retrieve a list of all news items.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                if (!_intranetApiContext.News.Any()) return NotFound(new List<News>());

                var news = _intranetApiContext.News
                    .Include(n => n.HeaderImage)
                    .Include(n => n.User)
                    .ToList();

                return new OkObjectResult(news);
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
                var news = _intranetApiContext.News.Find(id);

                if (news == null)
                {
                    return NotFound();
                }

                _intranetApiContext.Entry(news)
                    .Reference(n => n.HeaderImage)
                    .Load();

                _intranetApiContext.Entry(news)
                    .Reference(n => n.User)
                    .Load();

                return new OkObjectResult(news);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new News());
            }
        }
    }
}
