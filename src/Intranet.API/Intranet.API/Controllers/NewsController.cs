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
        [AllowAnonymous] // TODO this line is temporary for local testing without authentication, to be removed
        [HttpPost]
        public IActionResult Post([FromBody] News news)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var newNews = new News
                {
                    Title = news.Title,
                    Text = news.Text,
                    Author = news.Author
                };

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
        [AllowAnonymous] // TODO this line is temporary for local testing without authentication, to be removed
        [Route("{id:int}")]
        [HttpPut]
        public IActionResult Put(int id, [FromBody] News news)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

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
        [AllowAnonymous] // TODO this line is temporary for local testing without authentication, to be removed
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
        [AllowAnonymous] // TODO this line is temporary for local testing without authentication, to be removed
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                if (!_intranetApiContext.News.Any()) return NotFound(new List<News>());

                var searchResult = _intranetApiContext.News.ToList();
                return new OkObjectResult(searchResult);
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
        [AllowAnonymous] // TODO this line is temporary for local testing without authentication, to be removed
        [Route("{id:int}")]
        [HttpGet]
        public IActionResult Get(int id)
        {
            try
            {
                var fetchNewsById = _intranetApiContext.News.Find(id);

                if (fetchNewsById == null)
                {
                    return NotFound();
                }

                return new OkObjectResult(fetchNewsById);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new News());
            }
        }
    }
}