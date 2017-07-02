using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Intranet.API.Domain.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Intranet.API.Domain.Data;
using Intranet.API.ViewModels;

namespace Intranet.API.Controllers
{
    /// <summary>
    /// Manage tagging of news. For use
    /// with filtering of specific news types.
    /// </summary>
    [Produces("application/json")]
    [Route("/api/v1/[controller]")]
    public class NewsTagController : Controller
    {
        private readonly IntranetApiContext _intranetApiContext;

        public NewsTagController(IntranetApiContext intranetApiContext)
        {
            _intranetApiContext = intranetApiContext;
        }

        // TODO: Add unit tests
        /// <summary>
        /// Remove a tag from a news item
        /// </summary>
        /// <param name="newsId"></param>
        /// <param name="tagId"></param>
        /// <returns></returns>
        [Route("{newsId:int}/{tagId:int}")]
        [HttpDelete]
        public IActionResult Delete(int newsId, int tagId)
        {
            try
            {
                var newsTagToDelete = _intranetApiContext.NewsTags.SingleOrDefault(e => e.TagId == tagId && e.NewsId == newsId);

                if (newsTagToDelete == null)
                {
                    var error = Json("News tag can not be found");
                    return NotFound(error);
                }

                _intranetApiContext.NewsTags.Remove(newsTagToDelete);
                _intranetApiContext.SaveChanges();

                return Ok(newsTagToDelete);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // TODO: Add unit tests
        /// <summary>
        /// Retrieve a list of all tagged news
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var newsTags = _intranetApiContext.NewsTags.ToList();

                if (newsTags == null)
                {
                    var error = Json("Could not find any news tags");
                    return NotFound(error);
                }

                return Ok(newsTags);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new NewsTag());
            }
        }

        // TODO: Add unit tests
        /// <summary>
        /// Retrieve a list of tagged news
        /// filtered by tag id
        /// </summary>
        /// <param name="tagId"></param>
        /// <returns></returns>
        [Route("tag/{tagId:int}")]
        [HttpGet]
        public IActionResult Get(int tagId)
        {
            try
            {
                var newsByTag = _intranetApiContext.NewsTags.Where(e => e.TagId == tagId).ToList();

                if (newsByTag.Count == 0)
                {
                    var error = Json("Could not find any news tags");
                    return NotFound(error);
                }

                return Ok(newsByTag);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // TODO: Add unit tests
        /// <summary>
        /// Retrieve one specific news item with a specific tag.
        /// </summary>
        /// <param name="newsId"></param>
        /// <param name="tagId"></param>
        /// <returns></returns>
        [Route("{newsId:int}/{tagId:int}")]
        [HttpGet]
        public IActionResult Get(int newsId, int tagId)
        {
            try
            {
                if (newsId == 0 || tagId == 0)
                {
                    return BadRequest();
                }

                var newsTag = _intranetApiContext.NewsTags.SingleOrDefault(e => e.NewsId == newsId && e.TagId == tagId);

                if (newsTag == null)
                {
                    return NotFound();
                }

                return Ok(newsTag);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // TODO: Add unit tests
        /// <summary>
        /// Add a tag to a specific news item
        /// </summary>
        /// <param name="newsId"></param>
        /// <param name="tagId"></param>
        /// <returns></returns>
        [Route("{newsId:int}/{tagId:int}")]
        [HttpPost]
        public IActionResult Post(int newsId, int tagId)
        {
            try
            {
                var checkDuplicate = _intranetApiContext.NewsTags.SingleOrDefault(e => e.NewsId == newsId && e.TagId == tagId);

                if (checkDuplicate != null)
                {
                    ModelState.AddModelError("", "");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var newsTag = new NewsTag()
                {
                    NewsId = newsId,
                    TagId = tagId
                };

                _intranetApiContext.NewsTags.Add(newsTag);
                _intranetApiContext.SaveChanges();

                return Ok(ModelState);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // TODO: Add unit tests
        [HttpPut]
        public IActionResult Put(int firstId, int secondId, [FromBody] NewsTag body)
        {
            // TODO: Unsure how to handle PUT when the only changeable data are primary keys.
            throw new NotImplementedException();
        }
    }
}
