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
    [Produces("application/json")]
    [Route("/api/v1/[controller]")]
    public class LocationController : Controller, IRestController<Location> 
    {
        private readonly IntranetApiContext _intranetApiContext;

        public LocationController(IntranetApiContext intranetApiContext)
        {
            _intranetApiContext = intranetApiContext;
        }

        [AllowAnonymous]      // TODO this line is temporary for local testing without authentication, to be removed
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var locations = _intranetApiContext.Locations.ToList();

                if (locations.Count == 0)
                {
                    var error = Json("No location found");
                    return NotFound(error);
                }

                return Ok(locations);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [AllowAnonymous]      // TODO this line is temporary for local testing without authentication, to be removed
        [Route("{id:int}")]
        [HttpGet]
        public IActionResult Get(int id)
        {
            try
            {
                var location = _intranetApiContext.Locations.Find(id);

                if (location == null)
                {
                    var error = Json("No location found with id: " + id);
                    return NotFound(error);
                }

                return Ok(location);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [AllowAnonymous]      // TODO this line is temporary for local testing without authentication, to be removed
        [Route("{id:int}")]
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            try
            {
                var location = _intranetApiContext.Locations.Find(id);

                if (location == null)
                {
                    var error = Json("No location found with id: " + id);
                    return NotFound(error);
                }

                _intranetApiContext.Remove(location);
                _intranetApiContext.SaveChanges();

                return Ok(id);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [AllowAnonymous] // TODO this line is temporary for local testing without authentication, to be removed
        [HttpPost]
        public IActionResult Post([FromBody] Location body)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var newLocation = new Location
                {
                    Description = body.Description,
                    Coordinate = body.Coordinate
                };

                _intranetApiContext.Locations.Add(newLocation);
                _intranetApiContext.SaveChanges();

                return Ok(ModelState);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [AllowAnonymous]      // TODO this line is temporary for local testing without authentication, to be removed
        [Route("{id:int}")]
        [HttpPut]
        public IActionResult Put(int id, [FromBody] Location body)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var location = _intranetApiContext.Locations.Find(id);

                if (location == null)
                {
                    var error = Json("No location found with id: " + id);
                    return NotFound(error);
                }

                location.Description = body.Description;
                location.Coordinate = body.Coordinate;

                _intranetApiContext.SaveChanges();

                return Ok(ModelState);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}