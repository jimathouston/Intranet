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
    public class ClientController : Controller, IRestController<Client>  
    {
        private readonly IntranetApiContext _intranetApiContext;

        public ClientController(IntranetApiContext intranetApiContext)
        {
            _intranetApiContext = intranetApiContext;
        }

        [AllowAnonymous]      // TODO this line is temporary for local testing without authentication, to be removed
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var clients = _intranetApiContext.Clients.ToList();

                if (clients.Count == 0)
                {
                    var error = Json("No clients found");
                    return NotFound(error);
                }

                return Ok(clients);
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
                var client = _intranetApiContext.Clients.Find(id);

                if (client == null)
                {
                    var error = Json("No client found with id: " + id);
                    return NotFound(error);
                }

                return Ok(client);
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
                var client = _intranetApiContext.Clients.Find(id);

                if (client == null)
                {
                    var error = Json("No client found with id: " + id);
                    return NotFound(error);
                }

                _intranetApiContext.Remove(client);
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
        public IActionResult Post([FromBody] Client body)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var newClient = new Client
                {
                    Name = body.Name,
                    Description = body.Description,
                };

                _intranetApiContext.Clients.Add(newClient);
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
        public IActionResult Put(int id, [FromBody] Client body)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var client = _intranetApiContext.Clients.Find(id);

                if (client == null)
                {
                    var error = Json("No client found with id: " + id);
                    return NotFound(error);
                }

                client.Name = body.Name;
                client.Description = body.Description;

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