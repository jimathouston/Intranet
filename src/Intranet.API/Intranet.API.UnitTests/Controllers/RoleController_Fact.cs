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
    public class RoleController_Fact
    {
        [Fact]
        public void ReturnOkObjectWhenGetAllRoles()
        {
            // Assign
            var roles = GetStubRoles();
            DbContextFake.SeedDb<IntranetApiContext>(c => c.Roles.AddRange(roles), ensureDeleted: true);

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var controller = new RoleController(context);

                // Act
                var result = controller.Get();

                // Assert
                Assert.IsType<OkObjectResult>(result);
            };
        }

        [Fact]
        public void ReturnNotFoundWhenGetAllRoles()
        {
            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                // Assign
                context.Database.EnsureDeleted();
                var controller = new RoleController(context);

                // Act
                var result = controller.Get();

                // Assert
                var notFound = Assert.IsType<NotFoundObjectResult>(result);
                Assert.IsType<JsonResult>(notFound.Value);
            };
        }

        [Fact]
        public void ReturnOkObjectWhenGetRole()
        {
            // Assign
            var id = 1;
            var roles = GetStubRoles();

            DbContextFake.SeedDb<IntranetApiContext>(c => c.Roles.AddRange(roles), ensureDeleted: true);

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var controller = new RoleController(context);

                // Act
                var result = controller.Get(id);

                // Assert
                Assert.IsType<OkObjectResult>(result);
            }
        }

        [Fact]
        public void ReturnNotFoundWhenGetRole()
        {
            // Assign
            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                context.Database.EnsureDeleted();

                var id = 1;

                var controller = new RoleController(context);

                // Act
                var result = controller.Get(id);

                // Assert
                var notFound = Assert.IsType<NotFoundObjectResult>(result);
                Assert.IsType<JsonResult>(notFound.Value);
            }
        }

        [Fact]
        public void ReturnNotFoundWhenDeleteRole()
        {
            // Assign
            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                context.Database.EnsureDeleted();

                var id = 1;

                var controller = new RoleController(context);

                // Act
                var result = controller.Delete(id);

                // Assert
                var notFound = Assert.IsType<NotFoundObjectResult>(result);
                Assert.IsType<JsonResult>(notFound.Value);
            }
        }

        [Fact]
        public void ReturnOkObjectWhenDeleteRole()
        {
            // Assign
            var id = 1;
            var roles = GetStubRoles();

            DbContextFake.SeedDb<IntranetApiContext>(c => c.Roles.AddRange(roles), ensureDeleted: true);

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var controller = new RoleController(context);

                // Act
                var result = controller.Delete(id);

                // Assert
                var okObject = Assert.IsType<OkObjectResult>(result);
                Assert.IsType<int>(okObject.Value);
            }
        }

        [Fact]
        public void ReturnBadRequestWhenPostRole()
        {
            // Assign
            var role = GetStubRoles().First();

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                context.Database.EnsureDeleted();

                var controller = new RoleController(context);

                // Act
                controller.ModelState.AddModelError(nameof(Role.Description), "Role description name must be provided");
                var result = controller.Post(role);

                // Assert
                var badRequest = Assert.IsType<BadRequestObjectResult>(result);
                Assert.IsType<SerializableError>(badRequest.Value);
            }
        }

        [Fact]
        public void ReturnOkObjectWhenPostRole()
        {
            // Assign
            var role = GetStubRoles().First();

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                context.Database.EnsureDeleted();

                var controller = new RoleController(context);

                // Act
                var result = controller.Post(role);

                // Assert
                Assert.IsType<OkObjectResult>(result);
            }
        }

        [Fact]
        public void ReturnBadRequestWhenUpdateRole()
        {
            // Assign
            var id = 1;
            var roleToUpdate = GetStubRoles().First();

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                context.Database.EnsureDeleted();
                var controller = new RoleController(context);

                // Act
                controller.ModelState.AddModelError(nameof(Role.Description), "Role description must be provided");
                var result = controller.Put(id, roleToUpdate);

                // Assert
                var badRequest = Assert.IsType<BadRequestObjectResult>(result);
                Assert.IsType<SerializableError>(badRequest.Value);
            }
        }

        [Fact]
        public void ReturnNotFoundWhenUpdateRole()
        {
            // Assign
            var id = 1;
            var roleToUpdate = GetStubRoles().First();

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                context.Database.EnsureDeleted();
                var controller = new RoleController(context);

                // Act
                var result = controller.Put(id, roleToUpdate);

                // Assert
                var notFound = Assert.IsType<NotFoundObjectResult>(result);
                Assert.IsType<JsonResult>(notFound.Value);
            }
        }

        [Fact]
        public void ReturnOkObjectWhenUpdateRole()
        {
            // Assign
            var id = 1;
            var roleToUpdate = GetStubRoles().First();
            roleToUpdate.Description = "Project leader";
            var roles = GetStubRoles();

            DbContextFake.SeedDb<IntranetApiContext>(c => c.Roles.AddRange(roles), ensureDeleted: true);

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var controller = new RoleController(context);

                // Act
                var result = controller.Put(id, roleToUpdate);

                // Assert
                Assert.IsType<OkObjectResult>(result);
            }
        }

        private IEnumerable<Role> GetStubRoles()
        {
            return new Role[]
            {
                new Role
                {
                    Id = 1,
                    Description = "Component owner"
                },
                new Role
                {
                    Id = 2,
                    Description = "Tester"
                }
            };
        }
    }
}
