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
  public class ToDoController_Fact
  {
    [Fact]
    public void ReturnBadRequestWhenUpdate()
    {
      // Assign
      var options = new DbContextOptionsBuilder<IntranetApiContext>()
          .UseInMemoryDatabase(databaseName: nameof(ReturnBadRequestWhenUpdate))
          .Options;

      var context = new IntranetApiContext(options);
      context.Database.EnsureDeleted();
      context.SaveChanges();

      var id = 1;
      var updatedToDo = GetFakeToDos().First();
      updatedToDo.Description = "";

      var fakeToDos = GetFakeToDos();
      context.ToDos.AddRange(fakeToDos);
      context.SaveChanges();

      var controller = new ToDoController(context);

      // Act
      controller.ModelState.AddModelError(nameof(ToDo.Description), "Description must be specified");
      var result = controller.Put(id, updatedToDo);

      context.Dispose();

      // Assert
      Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public void ReturnNotFoundWhenUpdate()
    {
      // Assign
      var options = new DbContextOptionsBuilder<IntranetApiContext>()
          .UseInMemoryDatabase(databaseName: nameof(ReturnNotFoundWhenUpdate))
          .Options;

      var context = new IntranetApiContext(options);
      context.Database.EnsureDeleted();
      context.SaveChanges();

      var id = 9999;
      var fakeToDos = GetFakeToDos();
      var updatedToDo = GetFakeToDos().First();
      updatedToDo.Description = "";
      
      context.ToDos.AddRange(fakeToDos);
      context.SaveChanges();

      var controller = new ToDoController(context);

      // Act
      var result = controller.Put(id, updatedToDo);

      context.Dispose();

      // Assert
      Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public void ReturnOkResultWhenUpdateToDo()
    {
      // Assign
      var options = new DbContextOptionsBuilder<IntranetApiContext>()
          .UseInMemoryDatabase(databaseName: nameof(ReturnOkResultWhenUpdateToDo))
          .Options;

      var context = new IntranetApiContext(options);
      context.Database.EnsureDeleted();
      context.SaveChanges();

      var id = 1;
      var fakeToDos = GetFakeToDos();
      var updatedToDo = GetFakeToDos().First();
      updatedToDo.Description = "Test123456";

      context.ToDos.AddRange(fakeToDos);
      context.SaveChanges();

      var controller = new ToDoController(context);

      // Act
      var result = controller.Put(id, updatedToDo);

      context.Dispose();

      // Assert
      Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void ReturnBadRequestWhenPost()
    {
      // Assign
      var options = new DbContextOptionsBuilder<IntranetApiContext>()
          .UseInMemoryDatabase(databaseName: nameof(ReturnBadRequestWhenPost))
          .Options;

      var context = new IntranetApiContext(options);
      context.Database.EnsureDeleted();
      context.SaveChanges();

      var fakeToDo = GetFakeToDos().First();

      var controller = new ToDoController(context);

      // Act
      controller.ModelState.AddModelError(nameof(ToDo.Description), "Description must be provided");
      var result = controller.Post(fakeToDo);
      context.Dispose();

      // Assert
      Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public void ReturnOkResultWhenPost()
    {
      // Assign
      var options = new DbContextOptionsBuilder<IntranetApiContext>()
          .UseInMemoryDatabase(databaseName: nameof(ReturnOkResultWhenPost))
          .Options;

      var context = new IntranetApiContext(options);
      context.Database.EnsureDeleted();
      context.SaveChanges();

      var fakeToDo = GetFakeToDos().First();

      var controller = new ToDoController(context);
      
      // Act
      var result = controller.Post(fakeToDo);
      context.Dispose();

      // Assert
      Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void ReturnNotFoundWhenDelete()
    {
      // Assign
      var options = new DbContextOptionsBuilder<IntranetApiContext>()
          .UseInMemoryDatabase(databaseName: nameof(ReturnNotFoundWhenDelete))
          .Options;

      var context = new IntranetApiContext(options);
      context.Database.EnsureDeleted();
      context.SaveChanges();

      var id = 11231322;
      
      var fakeToDos = GetFakeToDos();
      context.ToDos.AddRange(fakeToDos);
      context.SaveChanges();

      var controller = new ToDoController(context);

      // Act
      var result = controller.Delete(id);
      context.Dispose();

      // Assert
      Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public void ReturnBadRequestWhenDelete()
    {
      // Assign
      var options = new DbContextOptionsBuilder<IntranetApiContext>()
          .UseInMemoryDatabase(databaseName: nameof(ReturnBadRequestWhenDelete))
          .Options;

      var context = new IntranetApiContext(options);
      context.Database.EnsureDeleted();
      context.SaveChanges();

      var id = 0;

      var fakeToDos = GetFakeToDos();
      context.ToDos.AddRange(fakeToDos);
      context.SaveChanges();

      var controller = new ToDoController(context);

      // Act
      var result = controller.Delete(id);

      context.Dispose();

      // Assert
      Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public void ReturnOkObjectResultWhenDelete()
    {
      // Assign
      var options = new DbContextOptionsBuilder<IntranetApiContext>()
          .UseInMemoryDatabase(databaseName: nameof(ReturnOkObjectResultWhenDelete))
          .Options;

      var context = new IntranetApiContext(options);
      context.Database.EnsureDeleted();
      context.SaveChanges();

      var id = 1;
      var fakeToDos = GetFakeToDos();
      context.ToDos.AddRange(fakeToDos);
      context.SaveChanges();

      var controller = new ToDoController(context);

      // Act
      var result = controller.Delete(id);

      context.Dispose();
      
      // Assert
      Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void ReturnNotFoundWhenGetTasks()
    {
      // Assign
      var options = new DbContextOptionsBuilder<IntranetApiContext>()
          .UseInMemoryDatabase(databaseName: nameof(ReturnNotFoundWhenGetTasks))
          .Options;

      var context = new IntranetApiContext(options);
      context.Database.EnsureDeleted();
      context.SaveChanges();

      var controller = new ToDoController(context);

      // Act
      var result = controller.Get();

      context.Dispose();

      // Assert
      Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void ReturnOkObjectWhenGetTasks()
    {
      // Assign
      var options = new DbContextOptionsBuilder<IntranetApiContext>()
          .UseInMemoryDatabase(databaseName: nameof(ReturnOkObjectWhenGetTasks))
          .Options;

      var context = new IntranetApiContext(options);
      context.Database.EnsureDeleted();
      context.ToDos.AddRange(GetFakeToDos());
      context.SaveChanges();

      var controller = new ToDoController(context);

      // Act
      var result = controller.Get();

      context.Dispose();

      // Assert
      Assert.IsType<OkObjectResult>(result);
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
          .UseInMemoryDatabase(databaseName: nameof(ReturnChecklistTaskById))
          .Options;

      var context = new IntranetApiContext(options);
      context.Database.EnsureDeleted();
      context.ToDos.AddRange(GetFakeToDos());
      context.SaveChanges();

      var controller = new ToDoController(context);

      // Act
      var result = controller.Get(taskId);
      var obj = result as ObjectResult;
      var toDo = obj.Value as ToDo;

      context.Dispose();

      // Assert
      Assert.Equal(toDo.Id == taskId, expected);
    }

    private IEnumerable<ToDo> GetFakeToDos()
    {
      return new ToDo[]
      {
        new ToDo
        {
          Id = 1,
          Description = "Read document with new employee instructions."
        },
        new ToDo
        {
          Id = 2,
          Description = "Obtain a mobile phone."
        },
        new ToDo
        {
          Id = 3,
          Description = "Obtain a computer."
        },
        new ToDo
        {
          Id = 4,
          Description = "Obtain an email address."
        },
        new ToDo
        {
          Id = 5,
          Description = "Submit your bank account details for salary."
        }
      };
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
  }
}
