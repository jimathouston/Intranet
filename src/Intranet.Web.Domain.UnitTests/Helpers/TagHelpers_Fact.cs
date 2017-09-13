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
    public class TagHelpers_Fact
    {
        [Fact]
        async public void No_Relations()
        {
            // Assign
            var result = false;

            DbContextFake.SeedDb<IntranetApiContext>(c => c.Add(new Tag()));

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                // Act
                result = await TagHelpers.HasNoRelatedEntities(context.Tags.SingleOrDefault(), context);
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
            var tag = new Tag
            {
                FaqTags = new List<FaqTag>
                {
                    new FaqTag(),
                },
            };

            DbContextFake.SeedDb<IntranetApiContext>(c => c.Tags.Add(tag));

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                // Act
                result = await TagHelpers.HasNoRelatedEntities(context.Tags.SingleOrDefault(), context);
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

            DbContextFake.SeedDb<IntranetApiContext>(c =>
            {
                c.Add(new Tag());
                c.Add(new Faq());
            });

            DbContextFake.SeedDb<IntranetApiContext>(c =>
            {
                c.Add(new FaqTag { Faq = c.Faqs.SingleOrDefault(), Tag = c.Tags.SingleOrDefault() });
            }, ensureDeleted: false);

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                // Act
                var tag = context.Tags.SingleOrDefault();
                await context
                    .Entry(tag)
                    .Collection(c => c.FaqTags)
                    .LoadAsync();

                result = await TagHelpers.HasNoRelatedEntities(tag, context, tag.FaqTags);
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
            var tag = new Tag
            {
                FaqTags = new List<FaqTag>
                {
                    new FaqTag(),
                    new FaqTag(),
                },
            };

            DbContextFake.SeedDb<IntranetApiContext>(c => c.Add(tag));

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                // Act
                result = await TagHelpers.HasNoRelatedEntities(context.Tags.SingleOrDefault(), context, new List<FaqTag> { tag.FaqTags.First() });
            }

            using (var context = DbContextFake.GetDbContext<IntranetApiContext>())
            {
                // Assert
                Assert.False(result);
            }
        }

        [Fact]
        public void Dont_Throw_On_Null()
        {
            // Assign

            // Act
            TagHelpers.SetTags<News, NewsTag>(null, null, null);
            TagHelpers.GetTagsFromString(null);

            // Assert
        }

        [Fact]
        public void Return_Array()
        {
            // Assign
            var tagsInput = "Test, test2; test-again";

            // Act
            var tags = TagHelpers.GetTagsFromString(tagsInput);

            // Assert
            Assert.Contains("Test", tags);
            Assert.Contains("test2", tags);
            Assert.Contains("test-again", tags);
        }

        [Fact]
        public void Create_New_Relationship()
        {
            // Assign
            var tags = new[] { "Test" };
            var news = new News();
            var faq = new Faq();

            // Act
            TagHelpers.SetTags<News, NewsTag>(tags, news, null);
            TagHelpers.SetTags<Faq, FaqTag>(tags, faq, null);

            // Assert
            Assert.Contains("Test", news.NewsTags.Select(nt => nt.Tag.Name));
            Assert.Contains("Test", faq.FaqTags.Select(nt => nt.Tag.Name));
        }

        [Fact]
        public void Attach_Old_Tag()
        {
            // Assign
            var tags = new[] { "Test" };
            var tag = new Tag
            {
                Id = 65,
                Name = "Test",
            };

            var news = new News();
            var faq = new Faq();

            // Act
            TagHelpers.SetTags<News, NewsTag>(tags, news, new List<Tag> { tag });
            TagHelpers.SetTags<Faq, FaqTag>(tags, faq, new List<Tag> { tag });

            // Assert
            Assert.Equal(0, news.NewsTags.Single().NewsId);
            Assert.Equal(0, faq.FaqTags.Single().FaqId);
            Assert.Equal(65, news.NewsTags.Single().Tag.Id);
            Assert.Equal(65, faq.FaqTags.Single().Tag.Id);
        }

        [Fact]
        public void Attach_Old_Relationship()
        {
            // Assign
            var tags = new[] { "Test" };

            var faq = new Faq { Id = 77 };
            var news = new News { Id = 33 };

            var faqTag = new FaqTag { FaqId = 77, Faq = faq, TagId = 65 };
            var newsTag = new NewsTag { NewsId = 33, News = news, TagId = 65 };

            var tag = new Tag
            {
                Id = 65,
                Name = "Test",
                NewsTags = new List<NewsTag> { newsTag },
                FaqTags = new List<FaqTag> { faqTag },
            };

            // Act
            TagHelpers.SetTags<News, NewsTag>(tags, news, new List<Tag> { tag });
            TagHelpers.SetTags<Faq, FaqTag>(tags, faq, new List<Tag> { tag });

            // Assert
            Assert.Equal(33, news.NewsTags.Single().NewsId);
            Assert.Equal(77, faq.FaqTags.Single().FaqId);
            Assert.Equal(65, faq.FaqTags.Single().TagId);
            Assert.Equal(65, news.NewsTags.Single().TagId);
        }

        [Fact]
        public void Attach_Old_Relationship_And_Create_New_Tags()
        {
            // Assign
            var tags = new[] { "Test", "Test2" };

            var faq = new Faq { Id = 77 };
            var news = new News { Id = 33 };

            var faqTag = new FaqTag { FaqId = 77, Faq = faq, TagId = 65 };
            var newsTag = new NewsTag { NewsId = 33, News = news, TagId = 65 };

            var tag = new Tag
            {
                Id = 65,
                Name = "Test",
                NewsTags = new List<NewsTag> { newsTag },
                FaqTags = new List<FaqTag> { faqTag },
            };

            faqTag.Tag = tag;
            newsTag.Tag = tag;

            // Act
            TagHelpers.SetTags<News, NewsTag>(tags, news, new List<Tag> { tag });
            TagHelpers.SetTags<Faq, FaqTag>(tags, faq, new List<Tag> { tag });

            // Assert
            Assert.Equal(33, news.NewsTags.Single(nt => nt.Tag.Name == "Test").NewsId);
            Assert.Equal(77, faq.FaqTags.Single(nt => nt.Tag.Name == "Test").FaqId);
            Assert.Equal(65, faq.FaqTags.Single(nt => nt.Tag.Name == "Test").TagId);
            Assert.Equal(65, news.NewsTags.Single(nt => nt.Tag.Name == "Test").TagId);
            Assert.Equal(0, faq.FaqTags.Single(nt => nt.Tag.Name == "Test2").Tag.Id);
            Assert.Equal(0, news.NewsTags.Single(nt => nt.Tag.Name == "Test2").Tag.Id);
        }
    }
}
