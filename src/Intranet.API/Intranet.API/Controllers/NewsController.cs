﻿using System;
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
  [Produces("application/json")]
  [Route("/api/v1/[controller]")]
  public class NewsController : Controller, IRestController<News>
  {
    private readonly IntranetApiContext _intranetApiContext;

    public NewsController(IntranetApiContext intranetApiContext)
    {
      _intranetApiContext = intranetApiContext;
    }

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

    //[AllowAnonymous] // TODO this line is temporary for local testing without authentication, to be removed
    //[HttpPut]
    [Route("{id}"), HttpPut, AllowAnonymous]
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

    [AllowAnonymous] // TODO this line is temporary for local testing without authentication, to be removed
    [HttpDelete("{id:int}")]
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

    [AllowAnonymous]      // TODO this line is temporary for local testing without authentication, to be removed
    [HttpGet]
    // GET api/news/ returns all available news
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

    [AllowAnonymous]      // TODO this line is temporary for local testing without authentication, to be removed
    [HttpGet("{id:int}")]
    // GET api/v1/news/5 returns a specific newsid
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