using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Intranet.API.Domain.Data;
using Intranet.API.Domain.Models.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Intranet.API.Data
{
  public static class DbInitializer
  {
    /// Check if DB is empty and seed it.
    /// source: http://stackoverflow.com/questions/34536021/seed-initial-data-in-entity-framework-7-rc-1-and-asp-net-mvc-6
    public static void SeedDb(IntranetApiContext context)
    {
      context.Database.EnsureCreated();

      var News = new News[]
      {
        new News
        {
          Date = new DateTimeOffset(new DateTime(2017, 04, 03)),
          Title = "Rubrik 1",
          Text = "Detta är en text till nyhet 1",
          Author = "Charlotta Utterström"
        },
        new News
        {
          Date = new DateTimeOffset(new DateTime(2017, 04, 02)),
          Title = "Rubrik 2",
          Text = "Detta är en text till nyhet 2",
          Author = "Charlotta Utterström"
        },
        new News
        {
          Date = new DateTimeOffset(new DateTime(2017, 03, 31)),
          Title = "Rubrik 3",
          Text = "Detta är en text till nyhet 3",
          Author = "Charlotta Utterström"
        },
        new News
        {
          Date = new DateTimeOffset(new DateTime(2017, 03, 30)),
          Title = "Rubrik 4",
          Text = "Detta är en text till nyhet 4",
          Author = "Charlotta Utterström"
        },
        new News
        {
          Date = new DateTimeOffset(new DateTime(2017, 03, 29)),
          Title = "Rubrik 5",
          Text = "Detta är en text till nyhet 5",
          Author = "Charlotta Utterström"
        }
      };

      context.News.AddRange(News);
      
      context.SaveChanges();
    }
  }
}
