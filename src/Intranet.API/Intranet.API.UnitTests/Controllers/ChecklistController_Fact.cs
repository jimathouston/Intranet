//using System;
//using System.Collections.Generic;
//using System.Text;
//using Xunit;
//using System.Linq;
//using Microsoft.EntityFrameworkCore;
//using Intranet.API.Controllers;
//using Intranet.API.Domain.Data;
//using Intranet.API.Domain.Models.Entities;
//using Microsoft.AspNetCore.Mvc;
//using Intranet.API.UnitTests.Fakes;

//namespace Intranet.API.UnitTests.Controllers
//{
//    public class ChecklistController_Fact
//    {

//        [Fact]
//        public void ReturnOkObjectWhenGetAll()
//        {
//            // Assign
//            int id = 1;
//            var employeeToDos = GetStubEmployeeToDos();
//            var employees = GetStubEmployees();
//            var todos = GetStubToDos();
//            DbContextFake.SeedDb<IntranetApiContext>(c => c.EmployeeToDos.AddRange(employeeToDos), ensureDeleted: true);
//            DbContextFake.SeedDb<IntranetApiContext>(c => c.Employees.AddRange(employees), ensureDeleted: false);
//            DbContextFake.SeedDb<IntranetApiContext>(c => c.ToDos.AddRange(todos), ensureDeleted: false);

//            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
//            {
//                var controller = new ChecklistController(context);

//                // Act
//                var result = controller.Get(id);

//                // Assert
//                Assert.IsType<OkObjectResult>(result);
//            };
//        }

//        [Fact]
//        public void ReturnBadRequestWhenGetAll()
//        {
//            // Assign
//            int id = 0;
//            var employeeToDos = GetStubEmployeeToDos();
//            var employees = GetStubEmployees();
//            var todos = GetStubToDos();
//            DbContextFake.SeedDb<IntranetApiContext>(c => c.EmployeeToDos.AddRange(employeeToDos), ensureDeleted: true);
//            DbContextFake.SeedDb<IntranetApiContext>(c => c.Employees.AddRange(employees), ensureDeleted: false);
//            DbContextFake.SeedDb<IntranetApiContext>(c => c.ToDos.AddRange(todos), ensureDeleted: false);

//            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
//            {
//                var controller = new ChecklistController(context);

//                // Act
//                var result = controller.Get(id);

//                // Assert
//                Assert.IsType<BadRequestResult>(result);
//            };
//        }

//        [Fact]
//        public void ReturnNotFoundWhenGetAll()
//        {
//            // Assign
//            int id = 21323;
//            var employeeToDos = GetStubEmployeeToDos();
//            var employees = GetStubEmployees();
//            var todos = GetStubToDos();
//            DbContextFake.SeedDb<IntranetApiContext>(c => c.EmployeeToDos.AddRange(employeeToDos), ensureDeleted: true);
//            DbContextFake.SeedDb<IntranetApiContext>(c => c.Employees.AddRange(employees), ensureDeleted: false);
//            DbContextFake.SeedDb<IntranetApiContext>(c => c.ToDos.AddRange(todos), ensureDeleted: false);

//            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
//            {
//                var controller = new ChecklistController(context);

//                // Act
//                var result = controller.Get(id);

//                // Assert
//                Assert.IsType<NotFoundResult>(result);
//            };

//        }

//        [Fact]
//        public void ReturnOkObjectWhenGetSingle()
//        {
//            // Assign
//            int profileId = 1;
//            int toDoId = 5;
//            var employeeToDos = GetStubEmployeeToDos();
//            var employees = GetStubEmployees();
//            var todos = GetStubToDos();
//            DbContextFake.SeedDb<IntranetApiContext>(c => c.EmployeeToDos.AddRange(employeeToDos), ensureDeleted: true);
//            DbContextFake.SeedDb<IntranetApiContext>(c => c.Employees.AddRange(employees), ensureDeleted: false);
//            DbContextFake.SeedDb<IntranetApiContext>(c => c.ToDos.AddRange(todos), ensureDeleted: false);

//            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
//            {
//                var controller = new ChecklistController(context);

//                // Act
//                var result = controller.Get(profileId, toDoId);

//                // Assert
//                Assert.IsType<OkObjectResult>(result);
//            };
//        }

//        [Fact]
//        public void ReturnBadRequestWhenGetSingle()
//        {
//            // Assign
//            int profileId = 0;
//            int toDoId = 5;
//            var employeeToDos = GetStubEmployeeToDos();
//            var employees = GetStubEmployees();
//            var todos = GetStubToDos();
//            DbContextFake.SeedDb<IntranetApiContext>(c => c.EmployeeToDos.AddRange(employeeToDos), ensureDeleted: true);
//            DbContextFake.SeedDb<IntranetApiContext>(c => c.Employees.AddRange(employees), ensureDeleted: false);
//            DbContextFake.SeedDb<IntranetApiContext>(c => c.ToDos.AddRange(todos), ensureDeleted: false);

//            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
//            {
//                var controller = new ChecklistController(context);

//                // Act
//                var result = controller.Get(profileId, toDoId);

//                // Assert
//                Assert.IsType<BadRequestResult>(result);
//            };
//        }

//        [Fact]
//        public void ReturnNotFoundWhenGetSingle()
//        {
//            // Assign
//            int profileId = 222221;
//            int toDoId = 225;
//            var employeeToDos = GetStubEmployeeToDos();
//            var employees = GetStubEmployees();
//            var todos = GetStubToDos();
//            DbContextFake.SeedDb<IntranetApiContext>(c => c.EmployeeToDos.AddRange(employeeToDos), ensureDeleted: true);
//            DbContextFake.SeedDb<IntranetApiContext>(c => c.Employees.AddRange(employees), ensureDeleted: false);
//            DbContextFake.SeedDb<IntranetApiContext>(c => c.ToDos.AddRange(todos), ensureDeleted: false);

//            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
//            {
//                var controller = new ChecklistController(context);

//                // Act
//                var result = controller.Get(profileId, toDoId);

//                // Assert
//                Assert.IsType<NotFoundResult>(result);
//            };
//        }

//        [Fact]
//        public void ReturnOkWhenPost()
//        {
//            // Assign
//            int id = 1;
//            var employees = GetStubEmployees();
//            var employeeToDo = GetStubEmployeeToDos().First();
//            var todos = GetStubToDos();
//            DbContextFake.SeedDb<IntranetApiContext>(c => c.Employees.AddRange(employees), ensureDeleted: false);
//            DbContextFake.SeedDb<IntranetApiContext>(c => c.ToDos.AddRange(todos), ensureDeleted: false);

//            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
//            {
//                var controller = new ChecklistController(context);

//                // Act
//                var result = controller.Post(id, employeeToDo);

//                // Assert
//                Assert.IsType<OkObjectResult>(result);
//            };
//        }

//        [Fact]
//        public void ReturnBadRequestWhenPost()
//        {
//            // Assign
//            int id = 1;
//            var employeeToDo = new EmployeeToDo();

//            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
//            {
//                var controller = new ChecklistController(context);

//                // Act
//                controller.ModelState.AddModelError(nameof(EmployeeToDo.EmployeeId), "Employee id must be specified");
//                var result = controller.Post(id, employeeToDo);

//                // Assert
//                Assert.IsType<BadRequestObjectResult>(result);
//            };
//        }

//        private IEnumerable<ToDo> GetStubToDos()
//        {
//            return new ToDo[]
//            {
//                new ToDo
//                {
//                    Id = 1,
//                    Description = "Read document with new employee instructions."
//                },
//                new ToDo
//                {
//                    Id = 2,
//                    Description = "Obtain a mobile phone."
//                },
//                new ToDo
//                {
//                    Id = 3,
//                    Description = "Obtain a computer."
//                },
//                new ToDo
//                {
//                    Id = 4,
//                    Description = "Obtain an email address."
//                },
//                new ToDo
//                {
//                    Id = 5,
//                    Description = "Submit your bank account details for salary."
//                }
//            };
//        }

//        private IEnumerable<EmployeeToDo> GetStubEmployeeToDos()
//        {
//            return new EmployeeToDo[]
//            {
//                new EmployeeToDo
//                    {
//                        EmployeeId = 1,
//                        ToDoId = 1,
//                        Done = false
//                    },
//                    new EmployeeToDo
//                    {
//                        EmployeeId = 1,
//                        ToDoId = 2,
//                        Done = true
//                    },
//                    new EmployeeToDo
//                    {
//                        EmployeeId = 1,
//                        ToDoId = 3,
//                        Done = false
//                    },
//                    new EmployeeToDo
//                    {
//                        EmployeeId = 1,
//                        ToDoId = 4,
//                        Done = false
//                    },
//                    new EmployeeToDo
//                    {
//                        EmployeeId = 1,
//                        ToDoId = 5,
//                        Done = false
//                    }
//            };
//        }

//        private IEnumerable<Employee> GetStubEmployees()
//        {
//            return new Employee[]
//            {
//                new Employee
//                    {
//                        Id = 1,
//                        FirstName = "Nils",
//                        LastName = "Nilsson",
//                        Description = "Consultant, new on the job and getting to grips with how things work.",
//                        Email = "nils.nilsson@certaincy.com",
//                        PhoneNumber = "1234-567890",
//                        Mobile = "1234-567890",
//                        StreetAdress = "Nils Nilssons Gata 1",
//                        PostalCode = 12345,
//                        City = "Gothenburg"
//                    }
//            };
//        }
//    }
//}
