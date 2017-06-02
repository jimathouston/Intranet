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
    public class LocationController_Fact
    {
        [Fact]
        public void ReturnOkObjectWhenGetAllLocations()
        {
            // Assign
            var locations = GetStubLocations();
            DbContextFake.SeedDb<IntranetApiContext>(c => c.Locations.AddRange(locations), ensureDeleted: true);

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var controller = new LocationController(context);

                // Act
                var result = controller.Get();

                // Assert
                Assert.IsType<OkObjectResult>(result);
            };
        }

        [Fact]
        public void ReturnNotFoundWhenGetAllLocations()
        {
            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                // Assign
                context.Database.EnsureDeleted();
                var controller = new LocationController(context);

                // Act
                var result = controller.Get();

                // Assert
                var notFound = Assert.IsType<NotFoundObjectResult>(result);
                Assert.IsType<JsonResult>(notFound.Value);
            };
        }

        [Fact]
        public void ReturnOkObjectWhenGetLocation()
        {
            // Assign
            var id = 1;
            var locations = GetStubLocations();

            DbContextFake.SeedDb<IntranetApiContext>(c => c.Locations.AddRange(locations), ensureDeleted: true);

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var controller = new LocationController(context);

                // Act
                var result = controller.Get(id);

                // Assert
                Assert.IsType<OkObjectResult>(result);
            }
        }

        [Fact]
        public void ReturnNotFoundWhenGetLocation()
        {
            // Assign
            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                context.Database.EnsureDeleted();

                var id = 1;

                var controller = new LocationController(context);

                // Act
                var result = controller.Get(id);

                // Assert
                var notFound = Assert.IsType<NotFoundObjectResult>(result);
                Assert.IsType<JsonResult>(notFound.Value);
            }
        }

        [Fact]
        public void ReturnNotFoundWhenDeleteLocation()
        {
            // Assign
            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                context.Database.EnsureDeleted();

                var id = 1;

                var controller = new LocationController(context);

                // Act
                var result = controller.Delete(id);

                // Assert
                var notFound = Assert.IsType<NotFoundObjectResult>(result);
                Assert.IsType<JsonResult>(notFound.Value);
            }
        }

        [Fact]
        public void ReturnOkObjectWhenDeleteLocation()
        {
            // Assign
            var id = 1;
            var locations = GetStubLocations();

            DbContextFake.SeedDb<IntranetApiContext>(c => c.Locations.AddRange(locations), ensureDeleted: true);

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var controller = new LocationController(context);

                // Act
                var result = controller.Delete(id);

                // Assert
                var okObject = Assert.IsType<OkObjectResult>(result);
                Assert.IsType<int>(okObject.Value);
            }
        }

        [Fact]
        public void ReturnBadRequestWhenPostLocation()
        {
            // Assign
            var location = GetStubLocations().First();

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                context.Database.EnsureDeleted();

                var controller = new LocationController(context);

                // Act
                controller.ModelState.AddModelError(nameof(Location.Description), "Description must be provided");
                var result = controller.Post(location);

                // Assert
                var badRequest = Assert.IsType<BadRequestObjectResult>(result);
                Assert.IsType<SerializableError>(badRequest.Value);
            }
        }

        [Fact]
        public void ReturnOkObjectWhenPostLocation()
        {
            // Assign
            var location = GetStubLocations().First();

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var controller = new LocationController(context);

                // Act
                var result = controller.Post(location);

                // Assert
                Assert.IsType<OkObjectResult>(result);
            }
        }

        [Fact]
        public void ReturnBadRequestWhenUpdateLocation()
        {
            // Assign
            var id = 1;
            var locationToUpdate = GetStubLocations().First();

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                context.Database.EnsureDeleted();
                var controller = new LocationController(context);

                // Act
                controller.ModelState.AddModelError(nameof(Location.Description), "Description must be provided");
                var result = controller.Put(id, locationToUpdate);

                // Assert
                var badRequest = Assert.IsType<BadRequestObjectResult>(result);
                Assert.IsType<SerializableError>(badRequest.Value);
            }
        }

        [Fact]
        public void ReturnNotFoundWhenUpdateLocation()
        {
            // Assign
            var id = 1;
            var locationToUpdate = GetStubLocations().First();

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                context.Database.EnsureDeleted();
                var controller = new LocationController(context);

                // Act
                var result = controller.Put(id, locationToUpdate);

                // Assert
                var notFound = Assert.IsType<NotFoundObjectResult>(result);
                Assert.IsType<JsonResult>(notFound.Value);
            }
        }

        [Fact]
        public void ReturnOkObjectWhenUpdateLocation()
        {
            // Assign
            var id = 1;
            var locationToUpdate = GetStubLocations().First();
            locationToUpdate.Description = "Volvo Plant, Torslanda";
            var locations = GetStubLocations();

            DbContextFake.SeedDb<IntranetApiContext>(c => c.Locations.AddRange(locations), ensureDeleted: true);

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var controller = new LocationController(context);

                // Act
                var result = controller.Put(id, locationToUpdate);

                // Assert
                Assert.IsType<OkObjectResult>(result);
            }
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
    }
}
