//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Intranet.API.Domain.Models.Entities;
//using Microsoft.AspNetCore.Authorization;
//using Intranet.API.Domain.Data;
//using Intranet.API.ViewModels;

//namespace Intranet.API.Controllers
//{
//    /// <summary>
//    /// Manage a list of available tags
//    /// that can be used to categorize news.
//    /// </summary>
//    [Produces("application/json")]
//    [Route("/api/v1/[controller]")]
//    public class TagController : Controller, IRestController<Tag>
//    {
//        private readonly IntranetApiContext _intranetApiContext;

//        public TagController(IntranetApiContext intranetApiContext)
//        {
//            _intranetApiContext = intranetApiContext;
//        }

//        // TODO: Add unit tests
//        /// <summary>
//        /// Retrieve a specific tag by id.
//        /// </summary>
//        /// <param name="id"></param>
//        /// <returns></returns>
//        [Route("{id:int}")]
//        [HttpGet]
//        public IActionResult Get(int id)
//        {
//            try
//            {
//                var tag = _intranetApiContext.Tags.Find(id);

//                if (tag == null)
//                {
//                    var error = Json("Could not find any tag with id: " + id);
//                    return NotFound(error);
//                }

//                return Ok(tag);
//            }
//            catch (Exception)
//            {
//                return StatusCode(StatusCodes.Status500InternalServerError, new Tag());
//            }
//        }

//        // TODO: Add unit tests
//        /// <summary>
//        /// Retrieve a list of all available tags.
//        /// </summary>
//        /// <returns></returns>
//        [HttpGet]
//        public IActionResult Get()
//        {
//            try
//            {
//                var tags = _intranetApiContext.Tags.ToList();

//                if (tags == null)
//                {
//                    var error = Json("Could not find any tags");
//                    return NotFound(error);
//                }

//                return Ok(tags);
//            }
//            catch (Exception)
//            {
//                return StatusCode(StatusCodes.Status500InternalServerError, new Tag());
//            }
//        }

//        // TODO: Add unit tests
//        /// <summary>
//        /// Add a new tag.
//        /// </summary>
//        /// <param name="tag"></param>
//        /// <returns></returns>
//        [HttpPost]
//        public IActionResult Post([FromBody] Tag tag)
//        {
//            try
//            {
//                if (!ModelState.IsValid)
//                {
//                    return BadRequest(ModelState);
//                }

//                var newTag = new Tag()
//                {
//                    Description = tag.Description
//                };

//                _intranetApiContext.Tags.Add(newTag);
//                _intranetApiContext.SaveChanges();

//                return Ok(ModelState);
//            }
//            catch (Exception)
//            {
//                return StatusCode(StatusCodes.Status500InternalServerError);
//            }
//        }

//        // TODO: Add unit tests
//        /// <summary>
//        /// Change the description of a specific tag.
//        /// </summary>
//        /// <param name="id"></param>
//        /// <param name="body"></param>
//        /// <returns></returns>
//        [Route("{id:int}")]
//        [HttpPut]
//        public IActionResult Put(int id, [FromBody] Tag body)
//        {
//            try
//            {
//                if (id == 0)
//                {
//                    ModelState.AddModelError(nameof(Tag.Id), "");
//                }

//                if (!ModelState.IsValid)
//                {
//                    return BadRequest(ModelState);
//                }

//                var updateTag = _intranetApiContext.Tags.Find(id);

//                if (updateTag == null)
//                {
//                    return NotFound(ModelState);
//                }

//                updateTag.Description = body.Description;

//                _intranetApiContext.SaveChanges();

//                return Ok(ModelState);
//            }
//            catch (Exception)
//            {
//                return StatusCode(StatusCodes.Status500InternalServerError);
//            }
//        }

//        // TODO: Add unit tests
//        /// <summary>
//        /// Remove a specific tag.
//        /// </summary>
//        /// <param name="id"></param>
//        /// <returns></returns>
//        [Route("{id:int}")]
//        [HttpDelete]
//        public IActionResult Delete(int id)
//        {
//            try
//            {
//                if (id == 0)
//                {
//                    return BadRequest(id);
//                }

//                var removeTag = _intranetApiContext.Tags.Find(id);

//                if (removeTag == null)
//                {
//                    var error = Json("Could not find any tag with id: " + id);
//                    return NotFound(error);
//                }

//                _intranetApiContext.Tags.Remove(removeTag);
//                _intranetApiContext.SaveChanges();

//                return Ok(id);
//            }
//            catch (Exception)
//            {
//                return StatusCode(StatusCodes.Status500InternalServerError);
//            }
//        }
//    }
//}
