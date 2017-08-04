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
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Intranet.Web.Common.Extensions;

namespace Intranet.Web.UnitTests.Controllers
{
    public class CategoriesController_Fact
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
                var categoryController = ControllerFake.GetController<CategoriesController>(context);

                // Act
                var response = await categoryController.Details(1);
                var returnedCategory = response.GetModelAs<Category>();

                // Assert
                Assert.IsType<ViewResult>(response);
                Assert.Equal("title", returnedCategory.Title);
            }
        }

        [Fact]
        public async Task Get_Category_Id_Should_Return_NotFound()
        {
            // Assign
            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var categoryController = ControllerFake.GetController<CategoriesController>(context);

                // Act
                var result = await categoryController.Details(1);

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
                var categoryController = ControllerFake.GetController<CategoriesController>(context);

                // Act
                var result = await categoryController.Index();
                var count = result.CountModelItems();

                // Assert
                Assert.IsType<ViewResult>(result);
                Assert.Equal(0, count);
            }
        }

        [Fact]
        public async Task Get_Categories_Should_Return_All_Categories()
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
                var categoryController = ControllerFake.GetController<CategoriesController>(context);

                // Act
                var result = await categoryController.Index();
                var responses = result.GetModelAs<IEnumerable<Category>>();
                var response = responses.First();

                // Assert
                Assert.IsType<ViewResult>(result);
                Assert.Equal(ActionResultExtensions.CountModelItems(result), 1);
                Assert.Equal(response.Title, "title");
            }
        }
        #endregion

        #region CREATE
        [Fact]
        public void Create_Should_Return_Empty_Category()
        {
            // Assign
            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var categoriesController = ControllerFake.GetController<CategoriesController>(context);

                // Act
                var response = categoriesController.Create();
                var category = response.GetModel();

                // Assert
                Assert.IsType<ViewResult>(response);
                Assert.IsType<Category>(category);
            }
        }

        [Fact]
        public async Task Create_Category_Should_Return_OkResult()
        {
            // Assign
            var category = new Category
            {
                Title = "title",
            };

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var categoryController = ControllerFake.GetController<CategoriesController>(context);

                // Act
                var response = await categoryController.Create(category);

                // Assert
                Assert.True(response.ValidateActionRedirect("Details"));
            }
            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                Assert.Equal("title", context.Categories.Single().Title);
                Assert.Equal("title", context.Categories.Single().Url);
            }
        }

        [Fact]
        public async Task Create_Duplicate_Category_Should_Return_Invalid_ModelState_With_Error_Message()
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
                var categoryController = ControllerFake.GetController<CategoriesController>(context);

                // Act
                var response = await categoryController.Create(category);
                var modelState = response.GetModelState();
                var errorMessage = response.GetModelStateErrorMessages("Error").SingleOrDefault();

                // Assert
                Assert.IsType<ViewResult>(response);
                Assert.False(modelState.IsValid);
                Assert.IsType<string>(errorMessage.ErrorMessage);
            }
        }
        #endregion

        #region EDIT
        [Fact]
        public async Task Get_Edit_Id_Should_Return_Category()
        {
            // Assign
            var faq = new Faq
            {
                Question = "q",
                Answer = "",
                Category = new Category
                {
                    Id = 1,
                    Title = "t",
                },
            };

            DbContextFake.SeedDb<IntranetApiContext>(c => c.Add(faq));

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var categoriesController = ControllerFake.GetController<CategoriesController>(context);

                // Act
                var result = await categoriesController.Edit(1);
                var returnedCategory = result.GetModelAs<Category>();

                // Assert
                Assert.IsType<ViewResult>(result);
                Assert.Equal(returnedCategory.Title, "t");
                Assert.Equal(returnedCategory.Faqs.Single().Question, "q");
            }
        }

        [Fact]
        public async Task Get_Edit_Id_Should_Return_NotFound_Category()
        {
            // Assign
            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var categoriesController = ControllerFake.GetController<CategoriesController>(context);

                // Act
                var result = await categoriesController.Edit(1);

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task Edit_Category_Should_Return_NotFound()
        {
            // Assign
            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var categoryController = ControllerFake.GetController<CategoriesController>(context);

                // Act
                var result = await categoryController.Edit(1, null);

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task Edit_Category_Should_Return_Ok()
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
                Id = 1,
                Title = "new category",
            };

            DbContextFake.SeedDb<IntranetApiContext>(c => c.Add(category));

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var categoryController = ControllerFake.GetController<CategoriesController>(context);

                // Act
                var result = await categoryController.Edit(1, newCategory);

                // Assert
                Assert.True(result.ValidateActionRedirect("Details"));
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
        public async Task Get_Delete_Id_Should_Return_Category()
        {
            // Assign
            var faq = new Faq
            {
                Id = 1,
                Question = "q",
                Answer = "",
                Category = new Category
                {
                    Title = "t",
                },
            };

            DbContextFake.SeedDb<IntranetApiContext>(c => c.Add(faq));

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var categoriesController = ControllerFake.GetController<CategoriesController>(context);

                // Act
                var result = await categoriesController.Delete(1);
                var returnedCategory = result.GetModelAs<Category>();

                // Assert
                Assert.IsType<ViewResult>(result);
                Assert.Equal(returnedCategory.Title, "t");
                Assert.Equal(returnedCategory.Faqs.Single().Question, "q");
            }
        }

        [Fact]
        public async Task Get_Delete_Id_Should_Return_NotFound()
        {
            // Assign
            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var categoriesController = ControllerFake.GetController<CategoriesController>(context);

                // Act
                var result = await categoriesController.Delete(1);

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

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
                var categoryController = ControllerFake.GetController<CategoriesController>(context);

                // Act
                var result = await categoryController.DeleteConfirmed(1);

                // Assert
                Assert.True(result.ValidateActionRedirect("Index"));
            }
            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                Assert.Equal(0, context.Categories.Count());
            }
        }

        [Fact]
        public async Task Delete_Category_Should_Return_Invalid_ModelState_With_Error_Message()
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
                var categoryController = ControllerFake.GetController<CategoriesController>(context);

                // Act
                var response = await categoryController.DeleteConfirmed(1);
                var modelState = response.GetModelState();
                var errorMessage = response.GetModelStateErrorMessages("Error").SingleOrDefault();

                // Assert
                Assert.IsType<ViewResult>(response);
                Assert.False(modelState.IsValid);
                Assert.IsType<string>(errorMessage.ErrorMessage);
            }
            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                Assert.Equal(1, context.Categories.Count());
            }
        }

        [Fact]
        public async Task Delete_Category_Should_Return_NotFound()
        {
            // Assign
            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var categoryController = ControllerFake.GetController<CategoriesController>(context);

                // Act
                var result = await categoryController.DeleteConfirmed(1);

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }
        #endregion
    }
}
