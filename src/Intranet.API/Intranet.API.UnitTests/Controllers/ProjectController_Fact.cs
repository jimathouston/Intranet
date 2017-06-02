using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Intranet.API.Controllers;
using Intranet.API.Domain.Data;
using Intranet.API.Domain.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Intranet.API.UnitTests.Fakes;

namespace Intranet.API.UnitTests.Controllers
{
    public class ProjectController_Fact
    {

        [Fact]
        public void ReturnOkObjectWhenGetAllProjects()
        {
            // Assign
            var projects = GetStubProjects();
            DbContextFake.SeedDb<IntranetApiContext>(c => c.Projects.AddRange(projects), ensureDeleted: true);

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var controller = new ProjectController(context);

                // Act
                var result = controller.Get();

                // Assert
                Assert.IsType<OkObjectResult>(result);
            };
        }

        [Fact]
        public void ReturnNotFoundWhenGetAllProjects()
        {
            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                // Assign
                context.Database.EnsureDeleted();
                var controller = new ProjectController(context);

                // Act
                var result = controller.Get();

                // Assert
                var notFound = Assert.IsType<NotFoundObjectResult>(result);
                Assert.IsType<JsonResult>(notFound.Value);
            };
        }

        [Fact]
        public void ReturnOkObjectWhenGetProject()
        {
            // Assign
            var id = 1;
            var projects = GetStubProjects();

            DbContextFake.SeedDb<IntranetApiContext>(c => c.Projects.AddRange(projects), ensureDeleted: true);

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var controller = new ProjectController(context);

                // Act
                var result = controller.Get(id);

                // Assert
                Assert.IsType<OkObjectResult>(result);
            }
        }

        [Fact]
        public void ReturnNotFoundWhenGetProject()
        {
            // Assign
            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                context.Database.EnsureDeleted();

                var id = 1;

                var controller = new ProjectController(context);

                // Act
                var result = controller.Get(id);

                // Assert
                var notFound = Assert.IsType<NotFoundObjectResult>(result);
                Assert.IsType<JsonResult>(notFound.Value);
            }
        }

        [Fact]
        public void ReturnNotFoundWhenDeleteProject()
        {
            // Assign
            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                context.Database.EnsureDeleted();

                var id = 1;

                var controller = new ProjectController(context);

                // Act
                var result = controller.Delete(id);

                // Assert
                var notFound = Assert.IsType<NotFoundObjectResult>(result);
                Assert.IsType<JsonResult>(notFound.Value);
            }
        }

        [Fact]
        public void ReturnOkObjectWhenDeleteProject()
        {
            // Assign
            var id = 1;
            var projects = GetStubProjects();

            DbContextFake.SeedDb<IntranetApiContext>(c => c.Projects.AddRange(projects), ensureDeleted: true);

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var controller = new ProjectController(context);

                // Act
                var result = controller.Delete(id);

                // Assert
                var okObject = Assert.IsType<OkObjectResult>(result);
                Assert.IsType<int>(okObject.Value);
            }
        }

        [Fact]
        public void ReturnBadRequestWhenPostProject()
        {
            // Assign
            var project = GetStubProjects().First();

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                context.Database.EnsureDeleted();

                var controller = new ProjectController(context);

                // Act
                controller.ModelState.AddModelError(nameof(Project.Name), "Project name must be provided");
                var result = controller.Post(project);

                // Assert
                var badRequest = Assert.IsType<BadRequestObjectResult>(result);
                Assert.IsType<SerializableError>(badRequest.Value);
            }
        }

        [Fact]
        public void ReturnBadReqWhenPostProjectWithNonexistingClientAndLocation()
        {
            // Assign
            var project = GetStubProjects().First();

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                context.Database.EnsureDeleted();

                var controller = new ProjectController(context);

                // Act
                var result = controller.Post(project);

                // Assert
                var badRequest = Assert.IsType<BadRequestObjectResult>(result);
                Assert.IsType<JsonResult>(badRequest.Value);
            }
        }

        [Fact]
        public void ReturnOkObjectWhenPostProject()
        {
            // Assign
            var project = GetStubProjects().First();
            var clients = GetStubClients();
            var locations = GetStubLocations();

            DbContextFake.SeedDb<IntranetApiContext>(c => c.Clients.AddRange(clients), ensureDeleted: true);
            DbContextFake.SeedDb<IntranetApiContext>(c => c.Locations.AddRange(locations), ensureDeleted: false);

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var controller = new ProjectController(context);

                // Act
                var result = controller.Post(project);

                // Assert
                Assert.IsType<OkObjectResult>(result);
            }
        }

        [Fact]
        public void ReturnBadRequestWhenUpdateProject()
        {
            // Assign
            var id = 1;
            var projectToUpdate = GetStubProjects().First();

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                context.Database.EnsureDeleted();
                var controller = new ProjectController(context);

                // Act
                controller.ModelState.AddModelError(nameof(Project.Name), "Project name must be provided");
                var result = controller.Put(id, projectToUpdate);

                // Assert
                var badRequest = Assert.IsType<BadRequestObjectResult>(result);
                Assert.IsType<SerializableError>(badRequest.Value);
            }
        }

        [Fact]
        public void ReturnNotFoundWhenUpdateProject()
        {
            // Assign
            var id = 1;
            var projectToUpdate = GetStubProjects().First();

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                context.Database.EnsureDeleted();
                var controller = new ProjectController(context);

                // Act
                var result = controller.Put(id, projectToUpdate);

                // Assert
                var notFound = Assert.IsType<NotFoundObjectResult>(result);
                Assert.IsType<JsonResult>(notFound.Value);
            }
        }

        [Fact]
        public void ReturnOkObjectWhenUpdateProject()
        {
            // Assign
            var id = 1;
            var projectToUpdate = GetStubProjects().First();
            projectToUpdate.Description = "Volvo test";
            var projects = GetStubProjects();
            var clients = GetStubClients();
            var locations = GetStubLocations();

            DbContextFake.SeedDb<IntranetApiContext>(c => c.Clients.AddRange(clients), ensureDeleted: true);
            DbContextFake.SeedDb<IntranetApiContext>(c => c.Locations.AddRange(locations), ensureDeleted: false);
            DbContextFake.SeedDb<IntranetApiContext>(c => c.Projects.AddRange(projects), ensureDeleted: false);


            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var controller = new ProjectController(context);

                // Act
                var result = controller.Put(id, projectToUpdate);

                // Assert
                Assert.IsType<OkObjectResult>(result);
            }
        }

        private IEnumerable<Project> GetStubProjects()
        {
            return new Project[]
            {
                new Project
                {
                    Id = 1,
                    Name = "HMMIOM digital dashboard / GTT",
                    Description = "Migration from mechanical to fully digital driver dashboard.",
                    ClientId = 1,
                    LocationId = 1
                },
                new Project
                {
                    Id = 2,
                    Name = "Web page development",
                    Description = "Development of new customer portal.",
                    ClientId = 3,
                    LocationId = 2
                }
            };
        }

        private IEnumerable<Location> GetStubLocations()
        {
            return new Location[]
            {
                new Location
                {
                    Id = 1,
                    Description = "GTT",
                    Coordinate = "40.714224,-73.961452"
                },
                new Location
                {
                    Id = 2,
                    Description = "Certaincy",
                    Coordinate = "37.423825,-122.082900"
                }
            };
        }

        private IEnumerable<Client> GetStubClients()
        {
            return new Client[]
            {
                new Client
                {
                    Id = 1,
                    Name = "Volvo Trucks",
                    Description = "Automotive company, manufacturer of heavy duty and light weight trucks."
                },
                new Client
                {
                    Id = 2,
                    Name = "Volvo Cars",
                    Description = "Automotive company, manufacturer of cars."
                },
                new Client
                {
                    Id = 3,
                    Name = "SCA",
                    Description = "Hygiene and forest products company."
                }
            };
        }
    }
}
