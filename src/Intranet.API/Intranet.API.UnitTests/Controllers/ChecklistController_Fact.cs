using Intranet.API.Controllers;
using Intranet.API.Domain.Data;
using Intranet.API.Domain.Models.Entities;
using Intranet.API.UnitTests.Mocks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Microsoft.EntityFrameworkCore;

namespace Intranet.API.UnitTests.Controllers
{
  public class ChecklistController_Fact
  {
    [Theory]
    [InlineData(1, true)]
    [InlineData(2, false)]
    public void CheckThatCorrectEmployeeIdIsReturned(int inputId, bool expected)
    {
      // Assign
      var options = new DbContextOptionsBuilder<IntranetApiContext>()
          .UseInMemoryDatabase()
          .Options;

      var context = new IntranetApiContext(options);
      context.Database.EnsureDeleted();

      var employeeData = GetFakeEmployee();
      context.Employees.AddRange(employeeData);

      var checklistData = GetFakeChecklist();
      context.ToDos.AddRange(checklistData);
      context.SaveChanges();

      var controller = new ChecklistController(context);

      // Act
      var employee = controller.GetEmployeeById(inputId);
      var obj = employee as ObjectResult;
      var employeeContent = obj.Value as Employee;

      context.Dispose();

      // Assert
      Assert.NotNull(obj);
      Assert.Equal(inputId == employeeContent.EmployeeId, expected);
    }

    [Theory]
    [InlineData(1, 200)]
    [InlineData(2, 404)]
    public void ReturnCorrectStatusCodeGetEmployeeById(int taskId, int statusCode)
    {
      // Assign
      var options = new DbContextOptionsBuilder<IntranetApiContext>()
          .UseInMemoryDatabase()
          .Options;

      var context = new IntranetApiContext(options);
      context.Database.EnsureDeleted();
      context.Employees.AddRange(GetFakeEmployee());
      context.ToDos.AddRange(GetFakeChecklist());
      context.SaveChanges();

      var controller = new ChecklistController(context);

      // Act
      var employee = controller.GetEmployeeById(taskId);
      var obj = employee as ObjectResult;

      context.Dispose();

      // Assert
      Assert.True(obj.StatusCode == statusCode);
    }

    [Theory]
    [InlineData(0, true)]
    [InlineData(1, true)]
    [InlineData(2, true)]
    [InlineData(4, true)]
    [InlineData(5, true)]
    [InlineData(6, false)]
    public void ReturnChecklistTaskById(int taskId, bool expected)
    {
      // Assign
      var options = new DbContextOptionsBuilder<IntranetApiContext>()
          .UseInMemoryDatabase()
          .Options;

      var context = new IntranetApiContext(options);
      context.Database.EnsureDeleted();
      context.Checklist.AddRange(GetFakeToDoTasks());
      context.SaveChanges();

      var controller = new ChecklistController(context);

      // Act
      var result = controller.GetChecklistTaskById(taskId);
      var obj = result as ObjectResult;
      var toDoTask = obj.Value as Checklist;

      context.Dispose();

      // Assert
      Assert.Equal(toDoTask.ChecklistId == taskId, expected);
    }

    private IEnumerable<Checklist> GetFakeToDoTasks()
    {
      return new Checklist[]
      {
        new Checklist
        {
          ChecklistId = 1,
          Description = "Read document with new employee instructions."
        },
        new Checklist
        {
          ChecklistId = 2,
          Description = "Obtain a mobile phone."
        },
        new Checklist
        {
          ChecklistId = 3,
          Description = "Obtain a computer."
        },
        new Checklist
        {
          ChecklistId = 4,
          Description = "Obtain an email address."
        },
        new Checklist
        {
          ChecklistId = 5,
          Description = "Submit your bank account details for salary."
        }
      };
    }

    private IEnumerable<ToDo> GetFakeChecklist()
    {
      return GetFakeChecklist(employeeId: 1);
    }

    private IEnumerable<ToDo> GetFakeChecklist(int employeeId)
    {
      return new ToDo[]
      {
        new ToDo
        {
          EmployeeId = employeeId,
          ChecklistId = 1,
          Done = false
        },
        new ToDo
        {
          EmployeeId = employeeId,
          ChecklistId = 2,
          Done = true
        },
        new ToDo
        {
          EmployeeId = employeeId,
          ChecklistId = 3,
          Done = false
        },
        new ToDo
        {
          EmployeeId = employeeId,
          ChecklistId = 4,
          Done = false
        },
        new ToDo
        {
          EmployeeId = employeeId,
          ChecklistId = 5,
          Done = false
        }
      };
    }

    private IEnumerable<Employee> GetFakeEmployee()
    {
      return GetFakeEmployee(id: 1);
    }

    private IEnumerable<Employee> GetFakeEmployee(int id)
    {
      return new Employee[]
      {
        new Employee
          {
            EmployeeId = id,
            FirstName = "Martin",
            LastName = "Norén",
            Description = "Likes cars!",
            Email = "noren.mar@gmail.com",
            PhoneNumber = "0702-111276",
            Mobile = "0702-111276",
            StreetAdress = "Jan Johanssons Gata 8",
            PostalCode = 41249,
            City = "Göteborg"
          }
      };
    }
  }
}
