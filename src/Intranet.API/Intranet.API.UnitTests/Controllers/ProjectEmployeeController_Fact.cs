//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Linq;
//using Microsoft.EntityFrameworkCore;
//using Intranet.API.Controllers;
//using Intranet.API.Domain.Data;
//using Intranet.API.Domain.Models.Entities;
//using Microsoft.AspNetCore.Mvc;
//using Intranet.API.UnitTests.Fakes;
//using Xunit;

//namespace Intranet.API.UnitTests.Controllers
//{
//    public class ProjectEmployeeController_Fact
//    {
//        [Fact]
//        public void ReturnOkWhenGetProjectEmployees()
//        {
//            // Assign
//            int id = 1;
//            var employee = GetStubEmployee();
//            var projects = GetStubProjects();
//            var roles = GetStubRoles();
//            var clients = GetStubClients();
//            var assignments = GetStubProjectEmployees();

//            DbContextFake.SeedDb<IntranetApiContext>(c => c.Projects.AddRange(projects), ensureDeleted: true);
//            DbContextFake.SeedDb<IntranetApiContext>(c => c.Roles.AddRange(roles), ensureDeleted: false);
//            DbContextFake.SeedDb<IntranetApiContext>(c => c.Employees.Add(employee), ensureDeleted: false);
//            DbContextFake.SeedDb<IntranetApiContext>(c => c.Clients.AddRange(clients), ensureDeleted: false);
//            DbContextFake.SeedDb<IntranetApiContext>(c => c.ProjectEmployees.AddRange(assignments), ensureDeleted: false);

//            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
//            {
//                var controller = new ProjectEmployeeController(context);

//                // Act
//                var result = controller.Get(id);

//                // Assert
//                Assert.IsType<OkObjectResult>(result);
//            };
//        }

//        [Fact]
//        public void ReturnNotFoundWhenGetProjectEmployees()
//        {
//            // Assign
//            int id = 1;

//            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
//            {
//                var controller = new ProjectEmployeeController(context);

//                // Act
//                var result = controller.Get(id);

//                // Assert
//                var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
//                Assert.IsType<JsonResult>(notFoundResult.Value);
//            };
//        }

//        [Fact]
//        public void ReturnOkWhenGetProjectEmployee()
//        {
//            // Assign
//            int id = 1;
//            int projectId = 4;
//            var employee = GetStubEmployee();
//            var projects = GetStubProjects();
//            var roles = GetStubRoles();

//            DbContextFake.SeedDb<IntranetApiContext>(c => c.Employees.Add(employee), ensureDeleted: true);
//            DbContextFake.SeedDb<IntranetApiContext>(c => c.Roles.AddRange(roles), ensureDeleted: false);
//            DbContextFake.SeedDb<IntranetApiContext>(c => c.Projects.AddRange(projects), ensureDeleted: false);

//            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
//            {
//                var controller = new ProjectEmployeeController(context);

//                // Act
//                var result = controller.Get(id, projectId);

//                // Assert
//                var jSonResult = Assert.IsType<NotFoundObjectResult>(result);
//                Assert.IsType<JsonResult>(jSonResult.Value);
//            };
//        }

//        [Fact]
//        public void ReturnNotFoundWhenGetProjectEmployee()
//        {
//            // Assign
//            int id = 1;
//            int projectId = 5;

//            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
//            {
//                var controller = new ProjectEmployeeController(context);

//                // Act
//                var result = controller.Get(id, projectId);

//                // Assert
//                var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
//                Assert.IsType<JsonResult>(notFoundResult.Value);
//            };
//        }

//        [Fact]
//        public void ReturnOkWhenPostProjectEmployee()
//        {
//            // Assign
//            int id = 1;
//            var employee = GetStubEmployee();
//            var projects = GetStubProjects();
//            var roles = GetStubRoles();
//            var clients = GetStubClients();

//            var assignment = GetStubProjectEmployees().First();

//            DbContextFake.SeedDb<IntranetApiContext>(c => c.Employees.Add(employee), ensureDeleted: true);
//            DbContextFake.SeedDb<IntranetApiContext>(c => c.Projects.AddRange(projects), ensureDeleted: false);
//            DbContextFake.SeedDb<IntranetApiContext>(c => c.Roles.AddRange(roles), ensureDeleted: false);
//            DbContextFake.SeedDb<IntranetApiContext>(c => c.Clients.AddRange(clients), ensureDeleted: false);

//            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
//            {
//                var controller = new ProjectEmployeeController(context);

//                // Act
//                var result = controller.Post(id, assignment);

//                // Assert
//                Assert.IsType<OkObjectResult>(result);
//            };
//        }

//        [Fact]
//        public void ReturnBadRequestWhenPostProjectEmployee()
//        {
//            // Assign
//            int id = 1;
//            var assignment = GetStubProjectEmployees().First();
//            var clients = GetStubClients();
//            var projects = GetStubProjects();
//            var employee = GetStubEmployee();
//            var roles = GetStubRoles();

//            DbContextFake.SeedDb<IntranetApiContext>(c => c.Clients.AddRange(clients), ensureDeleted: true);
//            DbContextFake.SeedDb<IntranetApiContext>(c => c.Projects.AddRange(projects), ensureDeleted: false);
//            DbContextFake.SeedDb<IntranetApiContext>(c => c.Roles.AddRange(roles), ensureDeleted: false);
//            DbContextFake.SeedDb<IntranetApiContext>(c => c.Employees.Add(employee), ensureDeleted: false);

//            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
//            {
//                var controller = new ProjectEmployeeController(context);

//                // Act
//                controller.ModelState.AddModelError(nameof(ProjectEmployee.RoleId), "RoleId must be provided");
//                var result = controller.Post(id, assignment);

//                // Assert
//                var badReqResult = Assert.IsType<BadRequestObjectResult>(result);
//                Assert.IsType<SerializableError>(badReqResult.Value);
//            };
//        }

//        [Fact]
//        public void ReturnOkWhenUpdateProjectEmployee()
//        {
//            // Assign
//            int id = 1;
//            int projectId = 1;
//            var employee = GetStubEmployee();
//            var projects = GetStubProjects();
//            var roles = GetStubRoles();
//            var clients = GetStubClients();
//            var oldEmployeeProject = GetStubProjectEmployees().First();
//            var newEmployeeProject = GetStubProjectEmployees().First();
//            newEmployeeProject.RoleId = 2;

//            DbContextFake.SeedDb<IntranetApiContext>(c => c.Employees.Add(employee), ensureDeleted: true);
//            DbContextFake.SeedDb<IntranetApiContext>(c => c.Projects.AddRange(projects), ensureDeleted: false);
//            DbContextFake.SeedDb<IntranetApiContext>(c => c.Roles.AddRange(roles), ensureDeleted: false);
//            DbContextFake.SeedDb<IntranetApiContext>(c => c.Clients.AddRange(clients), ensureDeleted: false);
//            DbContextFake.SeedDb<IntranetApiContext>(c => c.ProjectEmployees.Add(oldEmployeeProject), ensureDeleted: false);

//            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
//            {
//                var controller = new ProjectEmployeeController(context);

//                // Act
//                var result = controller.Put(id, projectId, newEmployeeProject);

//                // Assert
//                Assert.IsType<OkObjectResult>(result);
//            };
//        }

//        [Fact]
//        public void ReturnNotFoundWhenUpdateProjectEmployee()
//        {
//            // Assign
//            int id = 1;
//            int projectId = 1;
//            var projects = GetStubProjects();
//            var clients = GetStubClients();
//            var roles = GetStubRoles();
//            var employee = GetStubEmployee();
//            var assignment = GetStubProjectEmployees().First();
//            assignment.RoleId = 2;

//            DbContextFake.SeedDb<IntranetApiContext>(c => c.Projects.AddRange(projects), ensureDeleted: true);
//            DbContextFake.SeedDb<IntranetApiContext>(c => c.Roles.AddRange(roles), ensureDeleted: false);
//            DbContextFake.SeedDb<IntranetApiContext>(c => c.Clients.AddRange(clients), ensureDeleted: false);
//            DbContextFake.SeedDb<IntranetApiContext>(c => c.Employees.Add(employee), ensureDeleted: false);

//            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
//            {
//                var controller = new ProjectEmployeeController(context);

//                // Act
//                var result = controller.Put(id, projectId, assignment);

//                // Assert
//                var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
//                Assert.IsType<JsonResult>(notFoundResult.Value);
//            };
//        }

//        [Fact]
//        public void ReturnBadRequestWhenUpdateProjectEmployee()
//        {
//            // Assign
//            int id = 1;
//            int skillId = 1;
//            var projects = GetStubProjects();
//            var roles = GetStubRoles();
//            var employee = GetStubEmployee();
//            var newProjectEmployee = GetStubProjectEmployees().First();
//            newProjectEmployee.RoleId = 2;

//            DbContextFake.SeedDb<IntranetApiContext>(c => c.Projects.AddRange(projects), ensureDeleted: true);
//            DbContextFake.SeedDb<IntranetApiContext>(c => c.Roles.AddRange(roles), ensureDeleted: false);
//            DbContextFake.SeedDb<IntranetApiContext>(c => c.Employees.Add(employee), ensureDeleted: false);

//            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
//            {
//                var controller = new ProjectEmployeeController(context);

//                // Act
//                controller.ModelState.AddModelError("CurrentLevel", "Current skill level must be provided.");
//                var result = controller.Put(id, skillId, newProjectEmployee);

//                // Assert
//                var badReqResult = Assert.IsType<BadRequestObjectResult>(result);
//                Assert.IsType<SerializableError>(badReqResult.Value);
//            };
//        }

//        [Fact]
//        public void ReturnNotFoundWhenDeleteProjectEmployee()
//        {
//            // Assign
//            int id = 1;
//            int projectId = 1;

//            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
//            {
//                var controller = new ProjectEmployeeController(context);

//                // Act
//                var result = controller.Delete(id, projectId);

//                // Assert
//                var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
//                Assert.IsType<JsonResult>(notFoundResult.Value);
//            };
//        }

//        [Fact]
//        public void ReturnOkWhenDeleteProjectEmployee()
//        {
//            // Assign
//            int id = 1;
//            int projectId = 1;
//            var assignment = GetStubProjectEmployees().First();

//            DbContextFake.SeedDb<IntranetApiContext>(c => c.ProjectEmployees.Add(assignment), ensureDeleted: true);

//            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
//            {
//                var controller = new ProjectEmployeeController(context);

//                // Act
//                var result = controller.Delete(id, projectId);

//                // Assert
//                Assert.IsType<OkObjectResult>(result);
//            }
//        }

//        private Employee GetStubEmployee()
//        {
//            return new Employee
//            {
//                Id = 1,
//                FirstName = "Connie",
//                LastName = "Conniesson",
//                Description = "Consultant, socially active and time sensitive.",
//                Email = "connie.conniesson@certaincy.com",
//                PhoneNumber = "1234-567890",
//                Mobile = "1234-567890",
//                StreetAdress = "Connie Conniessons Gata 1",
//                PostalCode = 12345,
//                City = "Gothenburg"
//            };
//        }

//        private IEnumerable<Client> GetStubClients()
//        {
//            return new Client[]
//            {
//                new Client
//                {
//                    Id = 1,
//                    Name = "Volvo Trucks",
//                    Description = "Automotive company, manufacturer of heavy duty and light weight trucks."
//                },
//                new Client
//                {
//                    Id = 2,
//                    Name = "Volvo Cars",
//                    Description = "Automotive company, manufacturer of cars."
//                },
//                new Client
//                {
//                    Id = 3,
//                    Name = "SCA",
//                    Description = "Hygiene and forest products company."
//                }
//            };
//        }

//        private IEnumerable<Project> GetStubProjects()
//        {
//            return new Project[]
//            {
//                new Project
//                {
//                    Id = 1,
//                    Name = "HMMIOM digital dashboard / GTT",
//                    Description = "Migration from mechanical to fully digital driver dashboard.",
//                    ClientId = 1,
//                    LocationId = 1
//                },
//                new Project
//                {
//                    Id = 2,
//                    Name = "Web page development",
//                    Description = "Development of new customer portal.",
//                    ClientId = 3,
//                    LocationId = 2
//                }
//            };
//        }

//        private IEnumerable<Location> GetStubLocations()
//        {
//            return new Location[]
//            {
//                new Location
//                {
//                    Id = 1,
//                    Description = "GTT",
//                    Coordinate = "40.714224,-73.961452"
//                },
//                new Location
//                {
//                    Id = 2,
//                    Description = "Certaincy",
//                    Coordinate = "37.423825,-122.082900"
//                }
//            };
//        }

//        private IEnumerable<Role> GetStubRoles()
//        {
//            return new Role[]
//            {
//                new Role
//                {
//                    Id = 1,
//                    Description = "Component owner"
//                },
//                new Role
//                {
//                    Id = 2,
//                    Description = "Tester"
//                }
//            };
//        }

//        private IEnumerable<ProjectEmployee> GetStubProjectEmployees()
//        {
//            return new ProjectEmployee[]
//            {
//                new ProjectEmployee
//                {
//                    EmployeeId = 1,
//                    ProjectId = 1,
//                    RoleId = 1,
//                    Active = true,
//                    StartDate = DateTimeOffset.UtcNow,
//                    EndDate = DateTimeOffset.UtcNow,
//                    InformalDescription = "Component owner for HMIIOM ECU. X-functional responsibility between project, manufacturing and after market level to ensure successful deliveries and long term sustainability."
//                },
//                new ProjectEmployee
//                {
//                    EmployeeId = 2,
//                    ProjectId = 2,
//                    RoleId = 2,
//                    Active = false,
//                    StartDate = DateTimeOffset.UtcNow,
//                    EndDate = DateTimeOffset.UtcNow.AddMonths(4),
//                    InformalDescription = "Test developer for SCA web portal."
//                }
//            };
//        }
//    }
//}
