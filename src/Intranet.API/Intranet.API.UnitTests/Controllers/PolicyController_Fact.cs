using Intranet.API.Controllers;
using Intranet.API.Domain.Data;
using Intranet.API.Domain.Models.Entities;
using Intranet.API.UnitTests.Fakes;
using Intranet.API.UnitTests.TestHelpers;
using Intranet.API.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Intranet.API.UnitTests.Controllers
{
    public class PolicyController_Fact
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
                Category = new Category
                {
                    Title = "t",
                },
            };

            DbContextFake.SeedDb<IntranetApiContext>(c => c.Add(policy));

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var policyController = ControllerFake.GetController<PolicyController>(context);

                // Act
                var result = await policyController.GetAsync(1);
                var returnedPolicy = result.GetResponseAs<Policy>();

                // Assert
                Assert.IsType<OkObjectResult>(result);
                Assert.Equal(returnedPolicy.Title, "t");
                Assert.Equal(returnedPolicy.Category.Title, "t");
            }
        }

        [Fact]
        public async Task Get_Id_Should_Return_NotFound()
        {
            // Assign
            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var policyController = ControllerFake.GetController<PolicyController>(context);

                // Act
                var result = await policyController.GetAsync(1);

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task Get_All_Should_Return_Empty_List()
        {
            // Assign
            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var policyController = ControllerFake.GetController<PolicyController>(context);

                // Act
                var result = await policyController.GetAsync();

                // Assert
                Assert.IsType<OkObjectResult>(result);
                Assert.Equal(ActionResultHelpers.CountItems(result), 0);
            }
        }

        [Fact]
        public async Task Get_Should_Return_All_Policys()
        {
            // Assign
            var policy = new Policy
            {
                Id = 1,
                Title = "t",
                Category = new Category
                {
                    Title = "t",
                },
            };

            DbContextFake.SeedDb<IntranetApiContext>(c => c.Add(policy));

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var policyController = ControllerFake.GetController<PolicyController>(context);

                // Act
                var result = await policyController.GetAsync();
                var responses = result.GetResponsesAs<Policy>();
                var response = responses.First();

                // Assert
                Assert.IsType<OkObjectResult>(result);
                Assert.Equal(ActionResultHelpers.CountItems(result), 1);
                Assert.Equal(response.Title, "t");
                Assert.Equal(response.Category.Title, "t");
            }
        }
        #endregion

        #region POST
        [Fact]
        public async Task Post_Should_Return_OkResult()
        {
            // Assign
            var policy = new PolicyViewModel
            {
                Title = "tp",
                Category = new Category
                {
                    Title = "t",
                },
            };

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var policyController = ControllerFake.GetController<PolicyController>(context);

                // Act
                var response = await policyController.PostAsync(policy);

                // Assert
                Assert.IsType<OkObjectResult>(response);
            }
            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var savedPolicy = context.Policies.Include(f => f.Category).First();

                Assert.Equal(savedPolicy.Title, "tp");
                Assert.Equal(savedPolicy.Url, "tp");
                Assert.Equal(savedPolicy.Category.Title, "t");
                Assert.Equal(savedPolicy.Category.Url, "t");
            }
        }
        #endregion

        #region PUT
        [Fact]
        public async Task Put_Should_Return_NotFound()
        {
            // Assign
            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var policyController = ControllerFake.GetController<PolicyController>(context);

                // Act
                var result = await policyController.PutAsync(1, null);

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task Put_Should_Return_Ok()
        {
            // Assign
            var policy = new Policy
            {
                Id = 1,
                Title = "old policy",
                Url = "old-policy",
                Category = new Category
                {
                    Title = "old category",
                    Url = "old-category",
                },
            };

            var newPolicy = new PolicyViewModel
            {
                Title = "q",
                Category = new Category
                {
                    Title = "t",
                },
            };

            DbContextFake.SeedDb<IntranetApiContext>(c => c.Add(policy));

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var policyController = ControllerFake.GetController<PolicyController>(context);

                // Act
                var result = await policyController.PutAsync(1, newPolicy);

                // Assert
                Assert.IsType<OkObjectResult>(result);
            }
            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var updatedPolicy = context.Policies.Include(f => f.Category).First();

                Assert.Equal(policy.Title, "old policy");
                Assert.Equal(updatedPolicy.Title, "q");
                Assert.Equal(policy.Category.Title, "old category");
                Assert.Equal(updatedPolicy.Category.Title, "t");
                Assert.Equal(updatedPolicy.Url, "old-policy");
                Assert.Equal(updatedPolicy.Category.Url, "t");
            }
        }
        #endregion

        #region DELETE
        [Fact]
        public async Task Delete_Should_Return_OkResult()
        {
            // Assign
            var policy = new Policy
            {
                Id = 1,
                Title = "",
                Category = new Category
                {
                    Title = "",
                },
                PolicyKeywords = new List<PolicyKeyword>
                {
                    new PolicyKeyword
                    {
                        Keyword = new Keyword
                        {
                            Id = 1,
                        },
                    },
                },
            };

            DbContextFake.SeedDb<IntranetApiContext>(c => c.Add(policy));

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var policyController = ControllerFake.GetController<PolicyController>(context);

                // Act
                var result = await policyController.DeleteAsync(1);

                // Assert
                Assert.IsType<OkResult>(result);
            }
            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                Assert.Equal(0, context.Policies.Count());
                Assert.Equal(0, context.Categories.Count());
                Assert.Equal(0, context.Keywords.Count());
            }
        }

        [Fact]
        public async Task Delete_Should_Return_NotFound()
        {
            // Assign
            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var policyController = ControllerFake.GetController<PolicyController>(context);

                // Act
                var result = await policyController.DeleteAsync(1);

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }
        #endregion
    }
}
