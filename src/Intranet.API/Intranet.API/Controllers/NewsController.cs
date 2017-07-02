using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Intranet.API.Domain;
using Intranet.API.Domain.Data;
using Intranet.API.Domain.Models.Entities;
using Intranet.API.Data;
using Microsoft.AspNetCore.Authorization;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

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
                var newNews = new News
                {
                    Title = news.Title,
                    Text = news.Text,
                    Author = news.Author,
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
                var contextEntity = _intranetApiContext.News.Find(id);

                if (contextEntity == null)
                {
                    news.Id = id;
                    return NotFound(news);
                }

                contextEntity.Title = news.Title;
                contextEntity.Text = news.Text;
                contextEntity.Date = DateTimeOffset.UtcNow;
                contextEntity.Author = news.Author;
                if (contextEntity.HeaderImage != null) contextEntity.HeaderImage.FileName = news.HeaderImage?.FileName;

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
                var contextEntity = _intranetApiContext.News.Find(id);

                if (contextEntity == null)
                {
                    return NotFound();
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

                return new OkObjectResult(news);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new News());
            }
        }
    }
}
