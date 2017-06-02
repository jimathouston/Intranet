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
    public class ClientController_Fact
    {
        [Fact]
        public void ReturnOkObjectWhenGetAllClients()
        {
            // Assign
            var clients = GetStubClients();
            DbContextFake.SeedDb<IntranetApiContext>(c => c.Clients.AddRange(clients), ensureDeleted: true);

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var controller = new ClientController(context);

                // Act
                var result = controller.Get();

                // Assert
                Assert.IsType<OkObjectResult>(result);
            };
        }

        [Fact]
        public void ReturnNotFoundWhenGetAllClients()
        {
            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                // Assign
                context.Database.EnsureDeleted();
                var controller = new ClientController(context);

                // Act
                var result = controller.Get();

                // Assert
                var notFound = Assert.IsType<NotFoundObjectResult>(result);
                Assert.IsType<JsonResult>(notFound.Value);
            };
        }

        [Fact]
        public void ReturnOkObjectWhenGetClient()
        {
            // Assign
            var id = 1;
            var clients = GetStubClients();

            DbContextFake.SeedDb<IntranetApiContext>(c => c.Clients.AddRange(clients), ensureDeleted: true);

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var controller = new ClientController(context);

                // Act
                var result = controller.Get(id);

                // Assert
                Assert.IsType<OkObjectResult>(result);
            }
        }

        [Fact]
        public void ReturnNotFoundWhenGetClient()
        {
            // Assign
            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                context.Database.EnsureDeleted();

                var id = 1;

                var controller = new ClientController(context);

                // Act
                var result = controller.Get(id);

                // Assert
                var notFound = Assert.IsType<NotFoundObjectResult>(result);
                Assert.IsType<JsonResult>(notFound.Value);
            }
        }

        [Fact]
        public void ReturnNotFoundWhenDeleteClient()
        {
            // Assign
            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                context.Database.EnsureDeleted();

                var id = 1;

                var controller = new ClientController(context);

                // Act
                var result = controller.Delete(id);

                // Assert
                var notFound = Assert.IsType<NotFoundObjectResult>(result);
                Assert.IsType<JsonResult>(notFound.Value);
            }
        }

        [Fact]
        public void ReturnOkObjectWhenDeleteClient()
        {
            // Assign
            var id = 1;
            var clients = GetStubClients();

            DbContextFake.SeedDb<IntranetApiContext>(c => c.Clients.AddRange(clients), ensureDeleted: true);

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var controller = new ClientController(context);

                // Act
                var result = controller.Delete(id);

                // Assert
                var okObject = Assert.IsType<OkObjectResult>(result);
                Assert.IsType<int>(okObject.Value);
            }
        }

        [Fact]
        public void ReturnBadRequestWhenPostClient()
        {
            // Assign
            var client = GetStubClients().First();

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                context.Database.EnsureDeleted();

                var controller = new ClientController(context);

                // Act
                controller.ModelState.AddModelError(nameof(Client.Name), "Client name must be provided");
                var result = controller.Post(client);

                // Assert
                var badRequest = Assert.IsType<BadRequestObjectResult>(result);
                Assert.IsType<SerializableError>(badRequest.Value);
            }
        }

        [Fact]
        public void ReturnOkObjectWhenPostClient()
        {
            // Assign
            var client = GetStubClients().First();

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                context.Database.EnsureDeleted();

                var controller = new ClientController(context);

                // Act
                var result = controller.Post(client);

                // Assert
                Assert.IsType<OkObjectResult>(result);
            }
        }

        [Fact]
        public void ReturnBadRequestWhenUpdateClient()
        {
            // Assign
            var id = 1;
            var clientToUpdate = GetStubClients().First();
            
            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                context.Database.EnsureDeleted();
                var controller = new ClientController(context);

                // Act
                controller.ModelState.AddModelError(nameof(Client.Name), "Client name must be provided");
                var result = controller.Put(id, clientToUpdate);

                // Assert
                var badRequest = Assert.IsType<BadRequestObjectResult>(result);
                Assert.IsType<SerializableError>(badRequest.Value);
            }
        }

        [Fact]
        public void ReturnNotFoundWhenUpdateClient()
        {
            // Assign
            var id = 1;
            var clientToUpdate = GetStubClients().First();

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                context.Database.EnsureDeleted();
                var controller = new ClientController(context);

                // Act
                var result = controller.Put(id, clientToUpdate);

                // Assert
                var notFound = Assert.IsType<NotFoundObjectResult>(result);
                Assert.IsType<JsonResult>(notFound.Value);
            }
        }

        [Fact]
        public void ReturnOkObjectWhenUpdateClient()
        {
            // Assign
            var id = 1;
            var clientToUpdate = GetStubClients().First();
            clientToUpdate.Name = "Ericsson AB";
            var clients = GetStubClients();

            DbContextFake.SeedDb<IntranetApiContext>(c => c.Clients.AddRange(clients), ensureDeleted: true);

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var controller = new ClientController(context);

                // Act
                var result = controller.Put(id, clientToUpdate);

                // Assert
                Assert.IsType<OkObjectResult>(result);
            }
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
