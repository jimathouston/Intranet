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
//    /// Manage locations, which in turn are connected to where an employee has an assignment.
//    /// </summary>
//    [Produces("application/json")]
//    [Route("/api/v1/[controller]")]
//    public class LocationController : Controller, IRestController<Location> 
//    {
//        private readonly IntranetApiContext _intranetApiContext;

//        public LocationController(IntranetApiContext intranetApiContext)
//        {
//            _intranetApiContext = intranetApiContext;
//        }

//        /// <summary>
//        /// Retrieve a list of all locations.
//        /// </summary>
//        /// <returns></returns>
//        [HttpGet]
//        public IActionResult Get()
//        {
//            try
//            {
//                var locations = _intranetApiContext.Locations.ToList();

//                if (locations.Count == 0)
//                {
//                    var error = Json("No location found");
//                    return NotFound(error);
//                }

//                return Ok(locations);
//            }
//            catch (Exception)
//            {
//                return StatusCode(StatusCodes.Status500InternalServerError);
//            }
//        }

//        /// <summary>
//        /// Retrieve a specific location.
//        /// </summary>
//        /// <param name="id"></param>
//        /// <returns></returns>
//        [Route("{id:int}")]
//        [HttpGet]
//        public IActionResult Get(int id)
//        {
//            try
//            {
//                var location = _intranetApiContext.Locations.Find(id);

//                if (location == null)
//                {
//                    var error = Json("No location found with id: " + id);
//                    return NotFound(error);
//                }

//                return Ok(location);
//            }
//            catch (Exception)
//            {
//                return StatusCode(StatusCodes.Status500InternalServerError);
//            }
//        }

//        /// <summary>
//        /// Remove a location.
//        /// </summary>
//        /// <param name="id"></param>
//        /// <returns></returns>
//        [Route("{id:int}")]
//        [HttpDelete]
//        public IActionResult Delete(int id)
//        {
//            try
//            {
//                var location = _intranetApiContext.Locations.Find(id);

//                if (location == null)
//                {
//                    var error = Json("No location found with id: " + id);
//                    return NotFound(error);
//                }

//                _intranetApiContext.Remove(location);
//                _intranetApiContext.SaveChanges();

//                return Ok(id);
//            }
//            catch (Exception)
//            {
//                return StatusCode(StatusCodes.Status500InternalServerError);
//            }
//        }

//        /// <summary>
//        /// Add new location.
//        /// </summary>
//        /// <param name="body"></param>
//        /// <returns></returns>
//        [HttpPost]
//        public IActionResult Post([FromBody] Location body)
//        {
//            try
//            {
//                if (!ModelState.IsValid)
//                {
//                    return BadRequest(ModelState);
//                }

//                var newLocation = new Location
//                {
//                    Description = body.Description,
//                    Coordinate = body.Coordinate
//                };

//                _intranetApiContext.Locations.Add(newLocation);
//                _intranetApiContext.SaveChanges();

//                return Ok(ModelState);
//            }
//            catch (Exception)
//            {
//                return StatusCode(StatusCodes.Status500InternalServerError);
//            }
//        }

//        /// <summary>
//        /// Change content of a location.
//        /// Description and coordinates can be changed.
//        /// </summary>
//        /// <param name="id"></param>
//        /// <param name="body"></param>
//        /// <returns></returns>
//        [Route("{id:int}")]
//        [HttpPut]
//        public IActionResult Put(int id, [FromBody] Location body)
//        {
//            try
//            {
//                if (!ModelState.IsValid)
//                {
//                    return BadRequest(ModelState);
//                }

//                var location = _intranetApiContext.Locations.Find(id);

//                if (location == null)
//                {
//                    var error = Json("No location found with id: " + id);
//                    return NotFound(error);
//                }

//                location.Description = body.Description;
//                location.Coordinate = body.Coordinate;

//                _intranetApiContext.SaveChanges();

//                return Ok(ModelState);
//            }
//            catch (Exception)
//            {
//                return StatusCode(StatusCodes.Status500InternalServerError);
//            }
//        }
//    }
//}