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

namespace Intranet.API.Controllers
{
  [Produces("application/json")]
  [Route("/api/v1/[controller]")]
  public class NewsController : Controller
  {
    private readonly IntranetApiContext _intranetApiContext;

    public NewsController(IntranetApiContext intranetApiContext)
    {
      _intranetApiContext = intranetApiContext;
    }

    [AllowAnonymous]      // TODO this line is temporary for local testing without authentication, to be removed
    [HttpGet]
    // GET api/news/ returns all available news
    public IEnumerable<News> Get()
    {
      try
      {
        return _intranetApiContext.News.Any() ? _intranetApiContext.News.ToList() : new List<News>();
      }
      catch (Exception)
      {
        return new List<News>();
      }
    }

    [AllowAnonymous]      // TODO this line is temporary for local testing without authentication, to be removed
    [HttpGet("{id:int}")]
    // GET api/news/5 returns a specific newsid
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