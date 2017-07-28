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
    public class FaqController_Fact
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
                var faqController = ControllerFake.GetController<FaqController>(context);

                // Act
                var result = await faqController.GetAsync(1);
                var returnedFaq = result.GetResponseAs<Faq>();

                // Assert
                Assert.IsType<OkObjectResult>(result);
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
                var faqController = ControllerFake.GetController<FaqController>(context);

                // Act
                var result = await faqController.GetAsync(1);

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
                var faqController = ControllerFake.GetController<FaqController>(context);

                // Act
                var result = await faqController.GetAsync();

                // Assert
                Assert.IsType<OkObjectResult>(result);
                Assert.Equal(ActionResultHelpers.CountItems(result), 0);
            }
        }

        [Fact]
        public async Task Get_Should_Return_All_Faqs()
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
                var faqController = ControllerFake.GetController<FaqController>(context);

                // Act
                var result = await faqController.GetAsync();
                var responses = result.GetResponsesAs<Faq>();
                var response = responses.First();

                // Assert
                Assert.IsType<OkObjectResult>(result);
                Assert.Equal(ActionResultHelpers.CountItems(result), 1);
                Assert.Equal(response.Question, "q");
                Assert.Equal(response.Category.Title, "t");
            }
        }
        #endregion

        #region POST
        [Fact]
        public async Task Post_Should_Return_OkResult()
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
                var faqController = ControllerFake.GetController<FaqController>(context);

                // Act
                var response = await faqController.PostAsync(faq);

                // Assert
                Assert.IsType<OkObjectResult>(response);
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

        #region PUT
        [Fact]
        public async Task Put_Should_Return_NotFound()
        {
            // Assign
            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var faqController = ControllerFake.GetController<FaqController>(context);

                // Act
                var result = await faqController.PutAsync(1, null);

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task Put_Should_Return_Ok()
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
                var faqController = ControllerFake.GetController<FaqController>(context);

                // Act
                var result = await faqController.PutAsync(1, newFaq);

                // Assert
                Assert.IsType<OkObjectResult>(result);
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
        public async Task Delete_Should_Return_OkResult()
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
            };

            DbContextFake.SeedDb<IntranetApiContext>(c => c.Add(faq));

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var faqController = ControllerFake.GetController<FaqController>(context);

                // Act
                var result = await faqController.DeleteAsync(1);

                // Assert
                Assert.IsType<OkResult>(result);
            }
            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                Assert.Equal(context.Faqs.Count(), 0);
                Assert.Equal(context.Categories.Count(), 0);
            }
        }

        [Fact]
        public async Task Delete_Should_Return_NotFound()
        {
            // Assign
            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                var faqController = ControllerFake.GetController<FaqController>(context);

                // Act
                var result = await faqController.DeleteAsync(1);

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }
        #endregion
    }
}
