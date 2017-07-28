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
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Intranet.API.UnitTests.Controllers
{
    public class CategoryController_Fact
    {
        #region GET
        [Fact]
        public async Task Get_Category_Id_Should_Return_Category()
        {
            // Assign
            var category = new Category
            {
                Id = 1,
                Title = "title"
            };

            DbContextFake.SeedDb<IntranetApiContext>(c => c.Add(category));

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var categoryController = ControllerFake.GetController<CategoryController>(context);

                // Act
                var result = await categoryController.GetAsync(1);
                var returnedCategory = result.GetResponseAs<Category>();

                // Assert
                Assert.IsType<OkObjectResult>(result);
                Assert.Equal(returnedCategory.Title, "title");
            }
        }

        [Fact]
        public async Task Get_Category_Id_Should_Return_NotFound()
        {
            // Assign
            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var categoryController = ControllerFake.GetController<CategoryController>(context);

                // Act
                var result = await categoryController.GetAsync(1);

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task Get_All_Category_Should_Return_Empty_List()
        {
            // Assign
            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var categoryController = ControllerFake.GetController<CategoryController>(context);

                // Act
                var result = await categoryController.GetAsync();

                // Assert
                Assert.IsType<OkObjectResult>(result);
                Assert.Equal(ActionResultHelpers.CountItems(result), 0);
            }
        }

        [Fact]
        public async Task Get_Categories_Should_Return_All_Categorys()
        {
            // Assign
            var category = new Category
            {
                Id = 1,
                Title = "title",
            };

            DbContextFake.SeedDb<IntranetApiContext>(c => c.Add(category));

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var categoryController = ControllerFake.GetController<CategoryController>(context);

                // Act
                var result = await categoryController.GetAsync();
                var responses = result.GetResponsesAs<Category>();
                var response = responses.First();

                // Assert
                Assert.IsType<OkObjectResult>(result);
                Assert.Equal(ActionResultHelpers.CountItems(result), 1);
                Assert.Equal(response.Title, "title");
            }
        }
        #endregion

        #region POST
        [Fact]
        public async Task Post_Category_Should_Return_OkResult()
        {
            // Assign
            var category = new Category
            {
                Title = "title",
            };

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var categoryController = ControllerFake.GetController<CategoryController>(context);

                // Act
                var response = await categoryController.PostAsync(category);

                // Assert
                Assert.IsType<OkObjectResult>(response);
            }
            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                Assert.Equal("title", context.Categories.Single().Title);
                Assert.Equal("title", context.Categories.Single().Url);
            }
        }

        [Fact]
        public async Task Post_Duplicate_Category_Should_Return_BadRequestObjectResult_With_Error_Message()
        {
            // Assign
            var existingCategory = new Category
            {
                Title = "title",
            };

            var category = new Category
            {
                Title = "title",
            };

            DbContextFake.SeedDb<IntranetApiContext>(c => c.Add(existingCategory));

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var categoryController = ControllerFake.GetController<CategoryController>(context);

                // Act
                var response = await categoryController.PostAsync(category);
                var json = ActionResultHelpers.GetResponseAs<dynamic>(response);

                // Assert
                Assert.IsType<BadRequestObjectResult>(response);
                Assert.IsType<String>(json.GetType().GetProperty("Error").GetValue(json));
            }
        }
        #endregion

        #region PUT
        [Fact]
        public async Task Put_Category_Should_Return_NotFound()
        {
            // Assign
            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var categoryController = ControllerFake.GetController<CategoryController>(context);

                // Act
                var result = await categoryController.PutAsync(1, null);

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task Put_Category_Should_Return_Ok()
        {
            // Assign
            var category = new Category
            {
                Id = 1,
                Title = "old category",
                Url = "old-category",
            };

            var newCategory = new Category
            {
                Title = "new category",
            };

            DbContextFake.SeedDb<IntranetApiContext>(c => c.Add(category));

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var categoryController = ControllerFake.GetController<CategoryController>(context);

                // Act
                var result = await categoryController.PutAsync(1, newCategory);

                // Assert
                Assert.IsType<OkObjectResult>(result);
            }
            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var updatedCategory = context.Categories.Single();

                Assert.Equal(category.Title, "old category");
                Assert.Equal(category.Url, "old-category");
                Assert.Equal(updatedCategory.Title, "new category");
                Assert.Equal(updatedCategory.Url, "old-category");
            }
        }
        #endregion

        #region DELETE
        [Fact]
        public async Task Delete_Category_Should_Return_OkResult()
        {
            // Assign
            var category = new Category
            {
                Id = 1,
                Title = "",
            };

            DbContextFake.SeedDb<IntranetApiContext>(c => c.Add(category));

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var categoryController = ControllerFake.GetController<CategoryController>(context);

                // Act
                var result = await categoryController.DeleteAsync(1);

                // Assert
                Assert.IsType<OkResult>(result);
            }
            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                Assert.Equal(context.Categories.Count(), 0);
            }
        }

        [Fact]
        public async Task Delete_Category_Should_Return_BadRequestObjectResult_With_Error_Message()
        {
            // Assign
            var category = new Category
            {
                Id = 1,
                Title = "title",
                Faqs = new List<Faq>
                {
                    new Faq(),
                },
            };

            DbContextFake.SeedDb<IntranetApiContext>(c => c.Add(category));

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var categoryController = ControllerFake.GetController<CategoryController>(context);

                // Act
                var response = await categoryController.DeleteAsync(1);
                var json = ActionResultHelpers.GetResponseAs<dynamic>(response);

                // Assert
                Assert.IsType<BadRequestObjectResult>(response);
                Assert.IsType<String>(json.GetType().GetProperty("Error").GetValue(json));
            }
            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                Assert.Equal(context.Categories.Count(), 1);
            }
        }

        [Fact]
        public async Task Delete_Category_Should_Return_NotFound()
        {
            // Assign
            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var categoryController = ControllerFake.GetController<CategoryController>(context);

                // Act
                var result = await categoryController.DeleteAsync(1);

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }
        #endregion
    }
}
