using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Intranet.API.Controllers;
using Intranet.API.Domain.Data;
using Intranet.API.Domain.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Reflection;

namespace Intranet.API.UnitTests.Controllers
{
  public class ProfileController_Fact
  {
    [Fact]
    public void ReturnBadRequestWhenUpdate()
    {
      // Assign
      var options = new DbContextOptionsBuilder<IntranetApiContext>()
          .UseInMemoryDatabase(databaseName: "ProfileReturnBadRequestWhenUpdate")
          .Options;

      var employee = GetFakeEmployee().First();

      var context = new IntranetApiContext(options);
      context.Database.EnsureDeleted();
      context.Employees.Add(employee);
      context.SaveChanges();

      employee.FirstName = "";
      var id = 1;

      var employeeController = new ProfileController(context);

      // Act
      employeeController.ModelState.AddModelError("FirstName", "Firstname must be specified");
      var result = employeeController.Put(id, employee);

      // Assert
      var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
      Assert.IsType<SerializableError>(badRequestResult.Value);
    }

    [Fact]
    public void ReturnNotFoundWhenUpdate()
    {
      // Assign
      var options = new DbContextOptionsBuilder<IntranetApiContext>()
          .UseInMemoryDatabase(databaseName: "ProfileReturnNotFoundWhenUpdate")
          .Options;

      var id = 0;

      var employee = GetFakeEmployee().First();

      var context = new IntranetApiContext(options);
      context.Database.EnsureDeleted();

      var employeeController = new ProfileController(context);

      // Act
      var result = employeeController.Put(id, employee);
      context.Dispose();

      // Assert
      Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public void ReturnOkObjectWhenUpdating()
    {
      // Assign
      var options = new DbContextOptionsBuilder<IntranetApiContext>()
          .UseInMemoryDatabase(databaseName: "ProfileReturnOkObjectWhenUpdating")
          .Options;

      var oldEmployee = GetFakeEmployee().First();
      var newEmployee = GetFakeEmployee().First();
      newEmployee.FirstName = "Carl";

      var id = 1;

      var context = new IntranetApiContext(options);
      context.Database.EnsureDeleted();
      context.Employees.Add(oldEmployee);

      var employeeController = new ProfileController(context);

      // Act
      var result = employeeController.Put(id, newEmployee);
      context.Dispose();

      // Assert
      Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void VerifyProfileWasPostedCorrectly()
    {
      // Assign
      var options = new DbContextOptionsBuilder<IntranetApiContext>()
          .UseInMemoryDatabase(databaseName: "ProfileVerifyProfileWasPostedCorrectly")
          .Options;

      var fakeEmployee = GetFakeEmployee().First();

      var context = new IntranetApiContext(options);
      context.Database.EnsureDeleted();
      var employeeController = new ProfileController(context);

      // Act
      var result = employeeController.Post(fakeEmployee);
      var dbContent = context.Employees.First();
      context.Dispose();

      // Assert
      Assert.Equal(fakeEmployee.FirstName, dbContent.FirstName);
      Assert.Equal(fakeEmployee.LastName, dbContent.LastName);
      Assert.Equal(fakeEmployee.Description, dbContent.Description);
      Assert.Equal(fakeEmployee.Email, dbContent.Email);
      Assert.Equal(fakeEmployee.PhoneNumber, dbContent.PhoneNumber);
      Assert.Equal(fakeEmployee.Mobile, dbContent.Mobile);
      Assert.Equal(fakeEmployee.StreetAdress, dbContent.StreetAdress);
      Assert.Equal(fakeEmployee.PostalCode, dbContent.PostalCode);
      Assert.Equal(fakeEmployee.City, dbContent.City);
    }

    [Fact]
    public void ReturnOkObjectResultWhenPosting()
    {
      // Assign
      var options = new DbContextOptionsBuilder<IntranetApiContext>()
          .UseInMemoryDatabase(databaseName: "ProfileReturnOkObjectResultWhenPosting")
          .Options;

      var fakeEmployee = GetFakeEmployee().First();

      var context = new IntranetApiContext(options);
      context.Database.EnsureDeleted();
      var employeeController = new ProfileController(context);

      // Act
      var result = employeeController.Post(fakeEmployee);
      context.Dispose();

      //Assert
      Assert.IsType<OkObjectResult>(result);
    }

    [Theory]
    [InlineData("FirstName", "Firstname must be specified")]
    [InlineData("LastName", "LastName must be specified")]
    [InlineData("Email", "Email must be specified")]
    public void ReturnBadRequestResultWhenPosting(string errorType, string message)
    {
      // Assign
      var options = new DbContextOptionsBuilder<IntranetApiContext>()
          .UseInMemoryDatabase(databaseName: "ProfileReturnBadRequestResultWhenPosting")
          .Options;

      var fakeEmployee = GetFakeEmployee().First();

      var context = new IntranetApiContext(options);
      context.Database.EnsureDeleted();
      var employeeController = new ProfileController(context);

      // Act
      employeeController.ModelState.AddModelError(errorType, message);
      var result = employeeController.Post(fakeEmployee);
      context.Dispose();

      // Assert
      var badReqResult = Assert.IsType<BadRequestObjectResult>(result);
      Assert.IsType<SerializableError>(badReqResult.Value);
    }

    [Theory]
    [InlineData(1, 200)]
    [InlineData(2, 404)]
    [InlineData(0, 400)]
    public void ReturnCorrectStatusCodeDeleteById(int id, int statusCode)
    {
      // Assign
      var options = new DbContextOptionsBuilder<IntranetApiContext>()
          .UseInMemoryDatabase(databaseName: "ProfileReturnCorrectStatusCodeDeleteById")
          .Options;

      var fakeEmployee = GetFakeEmployee();

      var context = new IntranetApiContext(options);
      context.Database.EnsureDeleted();
      context.Employees.AddRange(fakeEmployee);
      context.SaveChanges();

      var controller = new ProfileController(context);

      // Act
      var result = controller.Delete(id);
      var obj = result as ObjectResult;
      context.Dispose();

      // Assert
      Assert.True(obj.StatusCode == statusCode);
    }

    [Fact]
    public void CheckThatEmployeeProfileIsDeleted()
    {
      // Assign
      var options = new DbContextOptionsBuilder<IntranetApiContext>()
          .UseInMemoryDatabase(databaseName: "ProfileCheckThatEmployeeProfileIsDeleted")
          .Options;

      int id = 1;
      var expectedCount = 0;

      var fakeEmployee = GetFakeEmployee();

      var context = new IntranetApiContext(options);
      context.Database.EnsureDeleted();
      context.Employees.AddRange(fakeEmployee);
      context.SaveChanges();

      var controller = new ProfileController(context);

      // Act
      var result = controller.Delete(id);
      var employees = context.Employees.ToList();
      context.Dispose();
      
      // Assert
      Assert.True(expectedCount == employees.Count);
    }

    [Theory]
    [InlineData(1, true)]
    [InlineData(2, false)]
    public void CheckThatCorrectEmployeeIdIsReturned(int id, bool expected)
    {
      // Assign
      var options = new DbContextOptionsBuilder<IntranetApiContext>()
          .UseInMemoryDatabase(databaseName: "ProfileCheckThatCorrectEmployeeIdIsReturned")
          .Options;

      var context = new IntranetApiContext(options);
      context.Database.EnsureDeleted();

      var employeeData = GetFakeEmployee();
      context.Employees.AddRange(employeeData);

      var checklistData = GetFakeChecklist();
      context.ToDos.AddRange(checklistData);
      context.SaveChanges();

      var controller = new ProfileController(context);

      // Act
      var employee = controller.Get(id);
      var obj = employee as ObjectResult;
      var employeeContent = obj.Value as Employee;

      context.Dispose();

      // Assert
      Assert.NotNull(obj);
      Assert.Equal(id == employeeContent.Id, expected);
    }

    [Theory]
    [InlineData(1, 200)]
    [InlineData(2, 404)]
    [InlineData(0, 400)]
    public void ReturnCorrectStatusCodeGetEmployeeById(int id, int statusCode)
    {
      // Assign
      var options = new DbContextOptionsBuilder<IntranetApiContext>()
          .UseInMemoryDatabase(databaseName: "ProfileReturnCorrectStatusCodeGetEmployeeById")
          .Options;

      var context = new IntranetApiContext(options);
      context.Database.EnsureDeleted();
      context.Employees.AddRange(GetFakeEmployee());
      context.ToDos.AddRange(GetFakeChecklist());
      context.SaveChanges();

      var controller = new ProfileController(context);

      // Act
      var employee = controller.Get(id);
      var obj = employee as ObjectResult;

      context.Dispose();

      // Assert
      Assert.True(obj.StatusCode == statusCode);
    }

    [Fact]
    public void ReturnNotFoundWhenGetAllEmployees()
    {
      // Assign
      var options = new DbContextOptionsBuilder<IntranetApiContext>()
          .UseInMemoryDatabase(databaseName: "ProfileReturnNotFoundWhenGetAllEmployees")
          .Options;

      var context = new IntranetApiContext(options);
      context.Database.EnsureDeleted();
      context.SaveChanges();

      var controller = new ProfileController(context);

      // Act
      var result = controller.Get();
      context.Dispose();

      // Assert
      Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public void ReturnOkObjectWhenGetAllEmployees()
    {
      // Assign
      var options = new DbContextOptionsBuilder<IntranetApiContext>()
          .UseInMemoryDatabase(databaseName: "ProfileReturnOkObjectWhenGetAllEmployees")
          .Options;

      var employee = GetFakeEmployee();

      var context = new IntranetApiContext(options);
      context.Database.EnsureDeleted();
      context.AddRange(employee);
      context.SaveChanges();

      var controller = new ProfileController(context);

      // Act
      var result = controller.Get();
      context.Dispose();

      // Assert
      Assert.IsType<OkObjectResult>(result);
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
            Id = id,
            FirstName = "Nils",
            LastName = "Nilsson",
            Description = "Consultant, new on the job and getting to grips with how things work.",
            Email = "nils.nilsson@certaincy.com",
            PhoneNumber = "1234-567890",
            Mobile = "1234-567890",
            StreetAdress = "Nils Nilssons Gata 1",
            PostalCode = 12345,
            City = "Gothenburg"
          }
      };
    }

    private Employee ChangeAPropertyField(Employee employee, string propertyToChange, string fieldValue)
    {
      if (!string.IsNullOrWhiteSpace(propertyToChange))
      {
        foreach (var item in employee.GetType().GetProperties())
        {
          if (item.Name == propertyToChange)
          {
            item.SetValue(employee, fieldValue);
          }
        }
      }

      return employee;
    }
  }
}
