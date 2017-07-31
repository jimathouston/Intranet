using Intranet.API.Domain.Data;
using Intranet.API.Domain.Helpers;
using Intranet.API.Domain.Models.Entities;
using Intranet.API.UnitTests.Fakes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Intranet.API.UnitTests.Domain.Helpers
{
    public class CategoryHelpers_Fact
    {
        [Fact]
        async public void No_Category_Relations()
        {
            // Assign
            var result = false;

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                // Act
                result = await CategoryHelpers.HasNoRelatedEntities(new Category(), context);
            }

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                // Assert
                Assert.True(result);
            }
        }

        [Fact]
        async public void Has_Category_Relations()
        {
            // Assign
            var result = true;
            var category = new Category
            {
                Faqs = new List<Faq>
                {
                    new Faq(),
                },
            };

            DbContextFake.SeedDb<IntranetApiContext>(c => c.Add(category));

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                // Act
                result = await CategoryHelpers.HasNoRelatedEntities(category, context);
            }

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                // Assert
                Assert.False(result);
            }
        }

        [Fact]
        async public void Ignore_Category_Relation_Has_No_Other()
        {
            // Assign
            var result = false;
            var category = new Category
            {
                Faqs = new List<Faq>
                {
                    new Faq(),
                },
            };

            DbContextFake.SeedDb<IntranetApiContext>(c => c.Add(category));

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                // Act
                result = await CategoryHelpers.HasNoRelatedEntities(category, context, category.Faqs.First());
            }

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                // Assert
                Assert.True(result);
            }
        }

        [Fact]
        async public void Ignore_Category_Relation_Has_Other()
        {
            // Assign
            var result = true;
            var category = new Category
            {
                Faqs = new List<Faq>
                {
                    new Faq(),
                    new Faq(),
                },
            };

            DbContextFake.SeedDb<IntranetApiContext>(c => c.Add(category));

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                // Act
                result = await CategoryHelpers.HasNoRelatedEntities(category, context, category.Faqs.First());
            }

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                // Assert
                Assert.False(result);
            }
        }
    }
}
