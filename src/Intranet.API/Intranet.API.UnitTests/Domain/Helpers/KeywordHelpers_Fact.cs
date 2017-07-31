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
    public class KeywordHelpers_Fact
    {
        [Fact]
        async public void No_Relations()
        {
            // Assign
            var result = false;

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                // Act
                result = await KeywordHelpers.HasNoRelatedEntities(new Keyword(), context);
            }

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                // Assert
                Assert.True(result);
            }
        }

        [Fact]
        async public void Has_Relations()
        {
            // Assign
            var result = true;
            var keyword = new Keyword
            {
                FaqKeywords = new List<FaqKeyword>
                {
                    new FaqKeyword(),
                },
            };

            DbContextFake.SeedDb<IntranetApiContext>(c => c.Add(keyword));

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                // Act
                result = await KeywordHelpers.HasNoRelatedEntities(keyword, context);
            }

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                // Assert
                Assert.False(result);
            }
        }

        [Fact]
        async public void Ignore_Relation_Has_No_Other()
        {
            // Assign
            var result = false;
            var keyword = new Keyword
            {
                FaqKeywords = new List<FaqKeyword>
                {
                    new FaqKeyword(),
                },
            };

            DbContextFake.SeedDb<IntranetApiContext>(c => c.Add(keyword));

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                // Act
                result = await KeywordHelpers.HasNoRelatedEntities(keyword, context, keyword.FaqKeywords);
            }

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                // Assert
                Assert.True(result);
            }
        }

        [Fact]
        async public void Ignore_Relation_Has_Other()
        {
            // Assign
            var result = true;
            var keyword = new Keyword
            {
                FaqKeywords = new List<FaqKeyword>
                {
                    new FaqKeyword(),
                    new FaqKeyword(),
                },
            };

            DbContextFake.SeedDb<IntranetApiContext>(c => c.Add(keyword));

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                // Act
                result = await KeywordHelpers.HasNoRelatedEntities(keyword, context, new List<FaqKeyword> { keyword.FaqKeywords.First() });
            }

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                // Assert
                Assert.False(result);
            }
        }
    }
}
