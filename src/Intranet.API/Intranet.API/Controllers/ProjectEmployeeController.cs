using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Intranet.API.Domain.Data;
using Intranet.API.Domain.Models.Entities;
using Microsoft.AspNetCore.Authorization;

namespace Intranet.API.Controllers
{
  [Produces("application/json")]
  [Route("/api/v1/[controller]")]
  public class ProjectEmployeeController : Controller, IRestController<ProjectEmployee>
  {
    private readonly IntranetApiContext _intranetApiContext;

    public ProjectEmployeeController(IntranetApiContext intranetApiContext)
    {
      _intranetApiContext = intranetApiContext;
    }

    [AllowAnonymous]      // TODO this line is temporary for local testing without authentication, to be removed
    [Route("{id:int}")]
    [HttpDelete]
    public IActionResult Delete(int id)
    {
      return StatusCode(StatusCodes.Status501NotImplemented);
    }

    [AllowAnonymous]      // TODO this line is temporary for local testing without authentication, to be removed
    [HttpGet]
    public IActionResult Get()
    {
      return StatusCode(StatusCodes.Status501NotImplemented);
    }

    [AllowAnonymous]      // TODO this line is temporary for local testing without authentication, to be removed
    [Route("{id:int}")]
    [HttpGet]
    public IActionResult Get(int id)
    {
      return StatusCode(StatusCodes.Status501NotImplemented);
    }

    [AllowAnonymous]      // TODO this line is temporary for local testing without authentication, to be removed
    [HttpPost]
    public IActionResult Post([FromBody] ProjectEmployee body)
    {
      return StatusCode(StatusCodes.Status501NotImplemented);
    }

    [AllowAnonymous]      // TODO this line is temporary for local testing without authentication, to be removed
    [Route("{id:int}")]
    [HttpPut]
    public IActionResult Put(int id, [FromBody] ProjectEmployee body)
    {
      return StatusCode(StatusCodes.Status501NotImplemented);
    }
  }
}