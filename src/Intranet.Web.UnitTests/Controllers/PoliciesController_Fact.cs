using Intranet.Web.Controllers;
using Intranet.Web.Domain.Data;
using Intranet.Web.Domain.Models.Entities;
using Intranet.Test.Tools.Fakes;
using Intranet.Test.Tools.Extensions;
using Intranet.Web.ViewModels;
using Intranet.Web.Common.Factories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using Xunit;
using System.Threading.Tasks;
using Intranet.Services.FileStorageService;

namespace Intranet.Web.UnitTests.Controllers
{
    public class PoliciesController_Fact
    {
        #region GET
        [Fact]
        public async Task Get_Id_Should_Return_Policy()
        {
            // Assign
            var policy = new Policy
            {
                Id = 1,
                Title = "t",
                Description = "d",
                Category = new Category
                {
                    Title = "c",
                },
            };
            var fileServiceMock = new Mock<IFileStorageService>();

            DbContextFake.SeedDb<IntranetApiContext>(c => c.Add(policy));

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var policyController = ControllerFake.GetController<PoliciesController>(context, fileServiceMock.Object);

                // Act
                var result = await policyController.Details(1);
                var returnedPolicy = result.GetModelAs<PolicyViewModel>();

                // Assert
                Assert.IsType<ViewResult>(result);
                Assert.Equal("t", returnedPolicy.Title);
                Assert.Equal("c", returnedPolicy.Category.Title);
            }
        }

        [Fact]
        public async Task Get_Id_Should_Return_NotFound()
        {
            // Assign
            var fileServiceMock = new Mock<IFileStorageService>();

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var policyController = ControllerFake.GetController<PoliciesController>(context, fileServiceMock.Object);

                // Act
                var result = await policyController.Details(1);

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task Get_All_Should_Return_Empty_List()
        {
            // Assign
            var fileServiceMock = new Mock<IFileStorageService>();

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var policyController = ControllerFake.GetController<PoliciesController>(context, fileServiceMock.Object);

                // Act
                var response = await policyController.Index();
                var result = response.GetModelAs<IEnumerable<Category>>();
                
                // Assert
                Assert.IsType<ViewResult>(response);
                Assert.Equal(0, result.Count());
            }
        }

        [Fact]
        public async Task Get_Should_Return_All_Policies_Grouped_By_Category()
        {
            // Assign
            var policy = new Policy
            {
                Id = 1,
                Title = "t", 
                Description = "d",
                Category = new Category
                {
                    Title = "c",
                },
            };
            var fileServiceMock = new Mock<IFileStorageService>();

            DbContextFake.SeedDb<IntranetApiContext>(c => c.Add(policy));

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var policyController = ControllerFake.GetController<PoliciesController>(context, fileServiceMock.Object);

                // Act
                var result = await policyController.Index();
                var categories = result.GetModelAs<IEnumerable<Category>>();
                var category = categories.First();

                // Assert
                Assert.IsType<ViewResult>(result);
                Assert.Equal(1, categories.Count());
                Assert.Equal("c",  category.Title);
                Assert.Equal("t", category.Policies.First().Title);
            }
        }
        #endregion

        #region CREATE
        [Fact]
        public void Create_Policy_Should_Return_Empty_View()
        {
            // Assign
            var fileServiceMock = new Mock<IFileStorageService>();

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var policyController = ControllerFake.GetController<PoliciesController>(context, fileServiceMock.Object);

                // Act
                var response = policyController.Create();
                var result = response.GetModel();

                // Assert
                Assert.IsType<ViewResult>(response);
                Assert.IsType<PolicyViewModel>(result);
            }
        }

        [Fact]
        public async Task Create_Policy_Should_Return_OkResult()
        {
            // Assign
            var policy = new PolicyViewModel
            {
                Title = "t", 
                Description = "d",
                Category = new Category
                {
                    Title = "c",
                },
            };
            var fileServiceMock = new Mock<IFileStorageService>();

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var policyController = ControllerFake.GetController<PoliciesController>(context, fileServiceMock.Object);

                // Act
                var response = await policyController.Create(policy);

                // Assert
                Assert.True(response.ValidateActionRedirect("Details"));
            }
            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var savedPolicy = context.Policies.Include(f => f.Category).First();

                Assert.Equal("t", savedPolicy.Title);
                Assert.Equal("t", savedPolicy.Url);
                Assert.Equal("c", savedPolicy.Category.Title);
                Assert.Equal("c", savedPolicy.Category.Url);
            }
        }
        #endregion

        #region UPDATE
        [Fact]
        public async Task Get_Edit_Id_Should_Return_Policy()
        {
            // Assign
            var policy = new Policy
            {
                Id = 1,
                Title = "t", 
                Description = "d",
                Category = new Category
                {
                    Title = "c",
                },
            };
            var fileServiceMock = new Mock<IFileStorageService>();

            DbContextFake.SeedDb<IntranetApiContext>(c => c.Add(policy));

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var policyController = ControllerFake.GetController<PoliciesController>(context, fileServiceMock.Object);

                // Act
                var result = await policyController.Edit(1);
                var returnedPolicy = result.GetModelAs<PolicyViewModel>();

                // Assert
                Assert.IsType<ViewResult>(result);
                Assert.Equal("t", returnedPolicy.Title);
                Assert.Equal("c", returnedPolicy.Category.Title);
            }
        }

        [Fact]
        public async Task Get_Edit_Id_Should_Return_NotFound()
        {
            // Assign
            var fileServiceMock = new Mock<IFileStorageService>();

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var policyController = ControllerFake.GetController<PoliciesController>(context, fileServiceMock.Object);

                // Act
                var result = await policyController.Edit(1);

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task Edit_Policy_Should_Return_NotFound()
        {
            // Assign
            var fileServiceMock = new Mock<IFileStorageService>();

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var policyController = ControllerFake.GetController<PoliciesController>(context, fileServiceMock.Object);

                // Act
                var result = await policyController.Edit(1, null);

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task Edit_Policy_Should_Return_Ok()
        {
            // Assign
            var policy = new Policy
            {
                Id = 1,
                Title = "old title",
                Description = "d",
                Url = "old-title",
                Category = new Category
                {
                    Title = "old category",
                    Url = "old-category",
                },
            };

            var newPolicy = new PolicyViewModel
            {
                Id = 1,
                Title = "t", 
                Description = "d",
                Category = new Category
                {
                    Title = "c",
                },
            };

            var fileServiceMock = new Mock<IFileStorageService>();

            DbContextFake.SeedDb<IntranetApiContext>(c => c.Add(policy));

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var policyController = ControllerFake.GetController<PoliciesController>(context, fileServiceMock.Object);

                // Act
                var result = await policyController.Edit(1, newPolicy);

                // Assert
                Assert.True(result.ValidateActionRedirect("Details"));
            }
            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var updatedPolicy = context.Policies.Include(f => f.Category).First();

                Assert.Equal("old title", policy.Title);
                Assert.Equal("t", updatedPolicy.Title);
                Assert.Equal("old category", policy.Category.Title);
                Assert.Equal("c", updatedPolicy.Category.Title);
                Assert.Equal("old-title", updatedPolicy.Url);
                Assert.Equal("c", updatedPolicy.Category.Url);
            }
        }
        #endregion

        #region DELETE
        [Fact]
        public async Task Get_Delete_Id_Should_Return_Policy()
        {
            // Assign
            var policy = new Policy
            {
                Id = 1,
                Title = "t", 
                Description = "d",
                Category = new Category
                {
                    Title = "c",
                },
            };
            var fileServiceMock = new Mock<IFileStorageService>();

            DbContextFake.SeedDb<IntranetApiContext>(c => c.Add(policy));

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var policyController = ControllerFake.GetController<PoliciesController>(context, fileServiceMock.Object);

                // Act
                var result = await policyController.Delete(1);
                var returnedPolicy = result.GetModelAs<PolicyViewModel>();

                // Assert
                Assert.IsType<ViewResult>(result);
                Assert.Equal("t",  returnedPolicy.Title);
                Assert.Equal("c",  returnedPolicy.Category.Title);
            }
        }

        [Fact]
        public async Task Get_Delete_Id_Should_Return_NotFound()
        {
            // Assign
            var fileServiceMock = new Mock<IFileStorageService>();

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var policyController = ControllerFake.GetController<PoliciesController>(context, fileServiceMock.Object);

                // Act
                var result = await policyController.Delete(1);

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task Delete_Policy_Should_Return_OkResult()
        {
            // Assign
            var policy = new Policy
            {
                Id = 1,
                Title = "",
                Description = "d",
                Category = new Category
                {
                    Title = "",
                },
                PolicyTags = new List<PolicyTag>
                {
                    new PolicyTag
                    {
                        Tag = new Tag
                        {
                            Id = 1,
                        },
                    },
                },
            };
            var fileServiceMock = new Mock<IFileStorageService>();

            DbContextFake.SeedDb<IntranetApiContext>(c => c.Add(policy));

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var policyController = ControllerFake.GetController<PoliciesController>(context, fileServiceMock.Object);

                // Act
                var result = await policyController.DeleteConfirmed(1);

                // Assert
                Assert.True(result.ValidateActionRedirect("Index"));
            }
            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                Assert.Equal(0, context.Policies.Count());
                Assert.Equal(0, context.Categories.Count());
                Assert.Equal(0, context.Tags.Count());
            }
        }

        [Fact]
        public async Task Delete_Should_Return_NotFound()
        {
            // Assign
            var fileServiceMock = new Mock<IFileStorageService>();

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var policyController = ControllerFake.GetController<PoliciesController>(context, fileServiceMock.Object);

                // Act
                var result = await policyController.DeleteConfirmed(1);

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }
        #endregion
    }
}
