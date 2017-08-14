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
    public class FaqsController_Fact
    {
        #region GET
        [Fact]
        public async Task Get_Id_Should_Return_Faq()
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
                var faqController = ControllerFake.GetController<FaqsController>(context);

                // Act
                var result = await faqController.Details(1);
                var returnedFaq = result.GetModelAs<FaqViewModel>();

                // Assert
                Assert.IsType<ViewResult>(result);
                Assert.Equal(returnedFaq.Question, "q");
                Assert.Equal(returnedFaq.Category.Title, "t");
            }
        }

        [Fact]
        public async Task Get_Id_Should_Return_NotFound()
        {
            // Assign
            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var faqController = ControllerFake.GetController<FaqsController>(context);

                // Act
                var result = await faqController.Details(1);

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
                var faqController = ControllerFake.GetController<FaqsController>(context);

                // Act
                var response = await faqController.Index();
                var result = response.GetModelAs<IEnumerable<Category>>();
                
                // Assert
                Assert.IsType<ViewResult>(response);
                Assert.Equal(0, result.Count());
            }
        }

        [Fact]
        public async Task Get_Should_Return_All_Faqs_Grouped_By_Category()
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
                var faqController = ControllerFake.GetController<FaqsController>(context);

                // Act
                var result = await faqController.Index();
                var categories = result.GetModelAs<IEnumerable<Category>>();
                var category = categories.First();

                // Assert
                Assert.IsType<ViewResult>(result);
                Assert.Equal(1, categories.Count());
                Assert.Equal("t", category.Title);
                Assert.Equal(category.Faqs.First().Question, "q");
            }
        }
        #endregion

        #region CREATE
        [Fact]
        public void Create_Should_Return_Empty_View()
        {
            // Assign
            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var faqController = ControllerFake.GetController<FaqsController>(context);

                // Act
                var response = faqController.Create();
                var result = response.GetModel();

                // Assert
                Assert.IsType<ViewResult>(response);
                Assert.IsType<FaqViewModel>(result);
            }
        }

        [Fact]
        public async Task Create_Should_Return_OkResult()
        {
            // Assign
            var faq = new FaqViewModel
            {
                Question = "q",
                Answer = "",
                Category = new Category
                {
                    Title = "t",
                },
            };

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var faqController = ControllerFake.GetController<FaqsController>(context);

                // Act
                var response = await faqController.Create(faq);

                // Assert
                Assert.True(response.ValidateActionRedirect("Details"));
            }
            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var savedFaq = context.Faqs.Include(f => f.Category).First();

                Assert.Equal(savedFaq.Question, "q");
                Assert.Equal(savedFaq.Url, "q");
                Assert.Equal(savedFaq.Category.Title, "t");
                Assert.Equal(savedFaq.Category.Url, "t");
            }
        }
        #endregion

        #region UPDATE
        [Fact]
        public async Task Get_Edit_Id_Should_Return_Faq()
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
                var faqController = ControllerFake.GetController<FaqsController>(context);

                // Act
                var result = await faqController.Edit(1);
                var returnedFaq = result.GetModelAs<FaqViewModel>();

                // Assert
                Assert.IsType<ViewResult>(result);
                Assert.Equal(returnedFaq.Question, "q");
                Assert.Equal(returnedFaq.Category.Title, "t");
            }
        }

        [Fact]
        public async Task Get_Edit_Id_Should_Return_NotFound()
        {
            // Assign
            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var faqController = ControllerFake.GetController<FaqsController>(context);

                // Act
                var result = await faqController.Edit(1);

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task Edit_Faq_Should_Return_NotFound()
        {
            // Assign
            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var faqController = ControllerFake.GetController<FaqsController>(context);

                // Act
                var result = await faqController.Edit(1, null);

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task Edit_Faq_Should_Return_Ok()
        {
            // Assign
            var faq = new Faq
            {
                Id = 1,
                Question = "old question",
                Answer = "",
                Url = "old-question",
                Category = new Category
                {
                    Title = "old category",
                    Url = "old-category",
                },
            };

            var newFaq = new FaqViewModel
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
                var faqController = ControllerFake.GetController<FaqsController>(context);

                // Act
                var result = await faqController.Edit(1, newFaq);

                // Assert
                Assert.True(result.ValidateActionRedirect("Details"));
            }
            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var updatedFaq = context.Faqs.Include(f => f.Category).First();

                Assert.Equal(faq.Question, "old question");
                Assert.Equal(updatedFaq.Question, "q");
                Assert.Equal(faq.Category.Title, "old category");
                Assert.Equal(updatedFaq.Category.Title, "t");
                Assert.Equal(updatedFaq.Url, "old-question");
                Assert.Equal(updatedFaq.Category.Url, "t");
            }
        }
        #endregion

        #region DELETE
        [Fact]
        public async Task Get_Delete_Id_Should_Return_Faq()
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
                var faqController = ControllerFake.GetController<FaqsController>(context);

                // Act
                var result = await faqController.Delete(1);
                var returnedFaq = result.GetModelAs<FaqViewModel>();

                // Assert
                Assert.IsType<ViewResult>(result);
                Assert.Equal("q", returnedFaq.Question);
                Assert.Equal("t", returnedFaq.Category.Title);
            }
        }

        [Fact]
        public async Task Get_Delete_Id_Should_Return_NotFound()
        {
            // Assign
            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var faqController = ControllerFake.GetController<FaqsController>(context);

                // Act
                var result = await faqController.Delete(1);

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task Delete_Faq_Should_Return_OkResult()
        {
            // Assign
            var faq = new Faq
            {
                Id = 1,
                Question = "",
                Answer = "",
                Category = new Category
                {
                    Title = "",
                },
                FaqTags = new List<FaqTag>
                {
                    new FaqTag
                    {
                        Tag = new Tag
                        {
                            Id = 1,
                        },
                    },
                },
            };

            DbContextFake.SeedDb<IntranetApiContext>(c => c.Add(faq));

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var faqController = ControllerFake.GetController<FaqsController>(context);

                // Act
                var result = await faqController.DeleteConfirmed(1);

                // Assert
                Assert.True(result.ValidateActionRedirect("Index"));
            }
            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                Assert.Equal(0, context.Faqs.Count());
                Assert.Equal(0, context.Categories.Count());
                Assert.Equal(0, context.Tags.Count());
            }
        }

        [Fact]
        public async Task Delete_Should_Return_NotFound()
        {
            // Assign
            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var faqController = ControllerFake.GetController<FaqsController>(context);

                // Act
                var result = await faqController.DeleteConfirmed(1);

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }
        #endregion
    }
}
