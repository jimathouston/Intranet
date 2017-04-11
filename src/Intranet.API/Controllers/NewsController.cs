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
using Newtonsoft.Json;
using System.Reflection;
using System.Net.Http;
using System.Net;

namespace Intranet.API.Controllers
{
  [Produces("application/json")]
  [Route("/api/[controller]")]
  public class NewsController : Controller
  {
    private readonly DomainModelPostgreSqlContext _newsContext;

    public NewsController(DomainModelPostgreSqlContext context)
    {
      _newsContext = context;
    }

    [AllowAnonymous]      // TODO this line is temporary for local testing without authentication, to be removed
    [HttpGet]
    // GET api/news/ returns all available news
    public IEnumerable<News> Get()
    {
      try
      {
        var fetchNews = _newsContext.News.ToList();
        foreach (var newsItem in fetchNews)
        {
          if (string.IsNullOrWhiteSpace(newsItem.Title))
          {
            newsItem.Title = "Title is missing.";
          }
          if (string.IsNullOrWhiteSpace(newsItem.Text))
          {
            newsItem.Text = "News context is missing.";
          }
          if (string.IsNullOrWhiteSpace(newsItem.Author))
          {
            newsItem.Author = "This news item is unsigned.";
          }
        }
        return fetchNews;
      }
      catch (Exception)
      {
        return null;      // TODO return something useful 
      }
    }

    [AllowAnonymous]      // TODO this line is temporary for local testing without authentication, to be removed
    [HttpGet("{id:int}")]
    // GET api/news/5 returns a specific newsid
    public IActionResult Get(int id)
    {
      var FetchSpecificNews = _newsContext.News.FirstOrDefault(n => n.Id == id);
     
      if (FetchSpecificNews == null)
      {
        return NotFound();
      }
      return new ObjectResult(FetchSpecificNews);
    }
  }
}