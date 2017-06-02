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
    /// Manage what projects are available and their content.
    /// </summary>
    [Produces("application/json")]
    [Route("/api/v1/[controller]")]
    public class ProjectController : Controller, IRestController<Project>   
    {
        private readonly IntranetApiContext _intranetApiContext;

        public ProjectController(IntranetApiContext intranetApiContext)
        {
            _intranetApiContext = intranetApiContext;
        }

        /// <summary>
        /// Retrieve a list of all projects and their content.
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]      // TODO this line is temporary for local testing without authentication, to be removed
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var projects = _intranetApiContext.Projects.ToList();

                if (projects.Count == 0)
                {
                    var error = Json("No projects found");
                    return NotFound(error);
                }

                return Ok(projects);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Retrieve a specific project and its content.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymous]      // TODO this line is temporary for local testing without authentication, to be removed
        [Route("{id:int}")]
        [HttpGet]
        public IActionResult Get(int id)
        {
            try
            {
                var project = _intranetApiContext.Projects.Find(id);

                if (project == null)
                {
                    var error = Json("No project found with id: " + id);
                    return NotFound(error);
                }

                return Ok(project);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Remove a specific project.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymous]      // TODO this line is temporary for local testing without authentication, to be removed
        [Route("{id:int}")]
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            try
            {
                var project = _intranetApiContext.Projects.Find(id);

                if (project == null)
                {
                    var error = Json("No project found with id: " + id);
                    return NotFound(error);
                }

                _intranetApiContext.Remove(project);
                _intranetApiContext.SaveChanges();

                return Ok(id);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Add a new project.
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [AllowAnonymous] // TODO this line is temporary for local testing without authentication, to be removed
        [HttpPost]
        public IActionResult Post([FromBody] Project body)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var client = _intranetApiContext.Clients.Find(body.ClientId);
                var location = _intranetApiContext.Locations.Find(body.LocationId);

                if (client == null || location == null)
                {
                    var error = Json("Client id or location id is incorrect");
                    return BadRequest(error);
                }

                var newProject = new Project
                {
                    Name = body.Name,
                    Description = body.Description,
                    ClientId = body.ClientId,
                    LocationId = body.LocationId
                };

                _intranetApiContext.Projects.Add(newProject);
                _intranetApiContext.SaveChanges();

                return Ok(ModelState);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Change content of a specific project.
        /// Name, description, client and location can be changed.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        [AllowAnonymous]      // TODO this line is temporary for local testing without authentication, to be removed
        [Route("{id:int}")]
        [HttpPut]
        public IActionResult Put(int id, [FromBody] Project body)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var project = _intranetApiContext.Projects.Find(id);

                if (project == null)
                {
                    var error = Json("No project found with id: " + id);
                    return NotFound(error);
                }

                var client = _intranetApiContext.Clients.Find(body.ClientId);
                var location = _intranetApiContext.Locations.Find(body.LocationId);

                if (client == null || location == null)
                {
                    var error = Json("Client id or location id is incorrect");
                    return BadRequest(error);
                }

                project.Name = body.Name;
                project.Description = body.Description;
                project.ClientId = body.ClientId;
                project.LocationId = body.LocationId;

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