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

      var news = new News[]
      {
        new News
        {
          Date = new DateTimeOffset(new DateTime(2017, 04, 03)),
          Title = "News title 1",
          Text = "This is a content placeholder for news title 1.",
          Author = "Charlotta Utterström"
        },
        new News
        {
          Date = new DateTimeOffset(new DateTime(2017, 04, 02)),
          Title = "News title  2",
          Text = "This is a content placeholder for news title 2",
          Author = "Charlotta Utterström"
        },
        new News
        {
          Date = new DateTimeOffset(new DateTime(2017, 03, 31)),
          Title = "News title  3",
          Text = "This is a content placeholder for news title 3",
          Author = "Charlotta Utterström"
        },
        new News
        {
          Date = new DateTimeOffset(new DateTime(2017, 03, 30)),
          Title = "News title  4",
          Text = "This is a content placeholder for news title 4",
          Author = "Charlotta Utterström"
        },
        new News
        {
          Date = new DateTimeOffset(new DateTime(2017, 03, 29)),
          Title = "News title  5",
          Text = "This is a content placeholder for news title 5",
          Author = "Charlotta Utterström"
        }
      };

      context.News.AddRange(news);
      context.SaveChanges();

      var checklist = new Checklist[]
      {
        new Checklist
        {
          Description = "Read document with new employee instructions."
        },
        new Checklist
        {
          Description = "Obtain a mobile phone."
        },
        new Checklist
        {
          Description = "Obtain a computer."
        },
        new Checklist
        {
          Description = "Obtain an email address."
        },
        new Checklist
        {
          Description = "Submit your bank account details for salary."
        }
      };

      context.Checklist.AddRange(checklist);
      context.SaveChanges();

      var employees = new Employee[]
      {
        new Employee
        {
          FirstName = "Martin",
          LastName = "Norén",
          Description = "Likes cars!",
          Email = "noren.mar@gmail.com",
          PhoneNumber = "0702-111276",
          Mobile = "0702-111276",
          StreetAdress = "Jan Johanssons Gata 8",
          PostalCode = 41249,
          City = "Göteborg"
        },
        new Employee
        {
          FirstName = "Carl",
          LastName = "Berg",
          Description = "Embedded systems specialist.",
          Email = "carl.b@gmail.com",
          PhoneNumber = "031-123456",
          Mobile = "1234-123456",
          StreetAdress = "Göteborgsvägen 1",
          PostalCode = 12345,
          City = "Göteborg"
        }
      };

      context.Employees.AddRange(employees);
      context.SaveChanges();

      var toDoList = new ToDo[]
    {
          new ToDo
          {
            EmployeeId = 1,
            ChecklistId = 1,
            Done = false
          },
          new ToDo
          {
            EmployeeId = 1,
            ChecklistId = 2,
            Done = true
          },
          new ToDo
          {
            EmployeeId = 1,
            ChecklistId = 3,
            Done = false
          },
          new ToDo
          {
            EmployeeId = 1,
            ChecklistId = 4,
            Done = false
          },
          new ToDo
          {
            EmployeeId = 1,
            ChecklistId = 5,
            Done = false
          },
          new ToDo
          {
            EmployeeId = 2,
            ChecklistId = 1,
            Done = true
          },
          new ToDo
          {
            EmployeeId = 2,
            ChecklistId = 2,
            Done = true
          },
          new ToDo
          {
            EmployeeId = 2,
            ChecklistId = 3,
            Done = true
          },
          new ToDo
          {
            EmployeeId = 2,
            ChecklistId = 4,
            Done = true
          },
          new ToDo
          {
            EmployeeId = 2,
            ChecklistId = 5,
            Done = true
          }
    };

      context.ToDos.AddRange(toDoList);
      context.SaveChanges();
    }
  }
}
