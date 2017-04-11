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
    public static void SeedData(this IApplicationBuilder app)
    {
      var _context = app.ApplicationServices.GetService<DomainModelPostgreSqlContext>();

      _context.Database.EnsureCreated();

      if (_context.News.Any())
      {
        return;
      }

      // Test dates/times generated from unix epoch time.
      // For online time converter see: http://www.freeformatter.com/epoch-timestamp-to-date-converter.html
      News[] NewNews = new News[]
      {
        new News
        {
          Date = DateTimeOffset.FromUnixTimeSeconds(1491177600),    // date is 2017-04-03 Time is 00:00:00 (UTC) and 02:00:00 when GMT+2
          Title = "Rubrik 1",
          Text = "Detta är en text till nyhet 1",
          Author = "Charlotta Utterström"
        },
        new News
        {
          Date = DateTimeOffset.FromUnixTimeSeconds(1491091200),    // date is 2017-04-02 Time is 00:00:00 (UTC)
          Title = "Rubrik 2",
          Text = "Detta är en text till nyhet 2",
          Author = "Charlotta Utterström"
        },
        new News
        {
          Date = DateTimeOffset.FromUnixTimeSeconds(1490918400),    // date is 2017-03-31 Time is 00:00:00 (UTC)
          Title = "Rubrik 3",
          Text = "Detta är en text till nyhet 3",
          Author = "Charlotta Utterström"
        },
        new News
        {
          Date = DateTimeOffset.FromUnixTimeSeconds(1490832000),    // date is 2017-03-30 Time is 00:00:00 (UTC)
          Title = "Rubrik 4",
          Text = "Detta är en text till nyhet 4",
          Author = "Charlotta Utterström"
        },
        new News
        {
          Date = DateTimeOffset.FromUnixTimeSeconds(1490745600),    // date is 2017-03-29 Time is 00:00:00 (UTC)
          Title = "Rubrik 5",
          Text = "Detta är en text till nyhet 5",
          Author = "Charlotta Utterström"
        }
      };

      foreach (News item in NewNews)
      {
        _context.News.Add(item);
      }

      _context.SaveChanges();
    }
  }
}
