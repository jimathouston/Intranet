using Intranet.Web.Domain.Data;
using Intranet.Web.Domain.Helpers;
using Intranet.Web.Domain.Models.Entities;
using Intranet.Test.Tools.Fakes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Intranet.Web.Domain.UnitTests.Helpers
{
    public class CategoryHelpers_Fact
    {
        [Fact]
        async public void No_Category_Relations()
        {
            // Assign
            var result = false;

            DbContextFake.SeedDb<IntranetApiContext>(c => c.Add(new Category()));

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                // Act
                result = await CategoryHelpers.HasNoRelatedEntities(context.Categories.SingleOrDefault(), context);
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
                result = await CategoryHelpers.HasNoRelatedEntities(context.Categories.SingleOrDefault(), context);
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

            DbContextFake.SeedDb<IntranetApiContext>(c => c.Add(new Category()));
            DbContextFake.SeedDb<IntranetApiContext>(c => c.Add(new Faq()), ensureDeleted: false);
            DbContextFake.SeedDb<IntranetApiContext>(c => c.Faqs.SingleOrDefault().Category = c.Categories.SingleOrDefault(), ensureDeleted: false);

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                // Act
                result = await CategoryHelpers.HasNoRelatedEntities(context.Categories.SingleOrDefault(), context, context.Faqs.SingleOrDefault());
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
                result = await CategoryHelpers.HasNoRelatedEntities(context.Categories.SingleOrDefault(), context, category.Faqs.First());
            }

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                // Assert
                Assert.False(result);
            }
        }
    }
}
