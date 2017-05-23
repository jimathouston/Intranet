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
using Intranet.API.ViewModels;

namespace Intranet.API.UnitTests.Controllers
{
  public class ProfileController_Fact
  {
    [Fact]
    public void ReturnBadRequestWhenUpdate()
    {
      // Assign
      var options = new DbContextOptionsBuilder<IntranetApiContext>()
          .UseInMemoryDatabase(databaseName: nameof(ReturnBadRequestWhenUpdate))
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
      employeeController.ModelState.AddModelError(nameof(Employee.FirstName), "Firstname must be specified");
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
          .UseInMemoryDatabase(databaseName: nameof(ReturnNotFoundWhenUpdate))
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
          .UseInMemoryDatabase(databaseName: nameof(ReturnOkObjectWhenUpdating))
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
          .UseInMemoryDatabase(databaseName: nameof(VerifyProfileWasPostedCorrectly))
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
          .UseInMemoryDatabase(databaseName: nameof(ReturnOkObjectResultWhenPosting))
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
          .UseInMemoryDatabase(databaseName: nameof(ReturnBadRequestResultWhenPosting))
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
          .UseInMemoryDatabase(databaseName: nameof(ReturnCorrectStatusCodeDeleteById))
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
          .UseInMemoryDatabase(databaseName: nameof(CheckThatEmployeeProfileIsDeleted))
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
          .UseInMemoryDatabase(databaseName: nameof(CheckThatCorrectEmployeeIdIsReturned))
          .Options;

      var context = new IntranetApiContext(options);
      context.Database.EnsureDeleted();

      var employeeData = GetFakeEmployee();
      context.Employees.AddRange(employeeData);

      var toDoList = GetFakeEmployeeToDoList();
      context.EmployeeToDos.AddRange(toDoList);
      context.SaveChanges();

      var controller = new ProfileController(context);

      // Act
      var employee = controller.Get(id);
      var obj = employee as ObjectResult;
      var employeeContent = obj.Value as ProfileViewModel;

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
          .UseInMemoryDatabase(databaseName: nameof(ReturnCorrectStatusCodeGetEmployeeById))
          .Options;

      var context = new IntranetApiContext(options);
      context.Database.EnsureDeleted();
      context.Employees.AddRange(GetFakeEmployee());
      context.EmployeeToDos.AddRange(GetFakeEmployeeToDoList());
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
          .UseInMemoryDatabase(databaseName: nameof(ReturnNotFoundWhenGetAllEmployees))
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
          .UseInMemoryDatabase(databaseName: nameof(ReturnOkObjectWhenGetAllEmployees))
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

    private IEnumerable<EmployeeToDo> GetFakeEmployeeToDoList()
    {
      return GetFakeEmployeeToDoList(employeeId: 1);
    }

    private IEnumerable<EmployeeToDo> GetFakeEmployeeToDoList(int employeeId)
    {
      return new EmployeeToDo[]
      {
        new EmployeeToDo
        {
          EmployeeId = employeeId,
          ToDoId = 1,
          Done = false
        },
        new EmployeeToDo
        {
          EmployeeId = employeeId,
          ToDoId = 2,
          Done = true
        },
        new EmployeeToDo
        {
          EmployeeId = employeeId,
          ToDoId = 3,
          Done = false
        },
        new EmployeeToDo
        {
          EmployeeId = employeeId,
          ToDoId = 4,
          Done = false
        },
        new EmployeeToDo
        {
          EmployeeId = employeeId,
          ToDoId = 5,
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
