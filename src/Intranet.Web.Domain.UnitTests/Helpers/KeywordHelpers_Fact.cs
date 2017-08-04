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

        [Fact]
        public void Dont_Throw_On_Null()
        {
            // Assign

            // Act
            KeywordHelpers.SetKeywords<News, NewsKeyword>(null, null, null);
            KeywordHelpers.GetKeywordsFromString(null);

            // Assert
        }

        [Fact]
        public void Return_Array()
        {
            // Assign
            var keywordsInput = "Test, test2; test-again";

            // Act
            var keywords = KeywordHelpers.GetKeywordsFromString(keywordsInput);

            // Assert
            Assert.Contains("Test", keywords);
            Assert.Contains("test2", keywords);
            Assert.Contains("test-again", keywords);
        }

        [Fact]
        public void Create_New_Relationship()
        {
            // Assign
            var keywords = new[] { "Test" };
            var news = new News();
            var faq = new Faq();

            // Act
            KeywordHelpers.SetKeywords<News, NewsKeyword>(keywords, news, null);
            KeywordHelpers.SetKeywords<Faq, FaqKeyword>(keywords, faq, null);

            // Assert
            Assert.Contains("Test", news.NewsKeywords.Select(nk => nk.Keyword.Name));
            Assert.Contains("Test", faq.FaqKeywords.Select(nk => nk.Keyword.Name));
        }

        [Fact]
        public void Attach_Old_Keyword()
        {
            // Assign
            var keywords = new[] { "Test" };
            var keyword = new Keyword
            {
                Id = 65,
                Name = "Test",
            };

            var news = new News();
            var faq = new Faq();

            // Act
            KeywordHelpers.SetKeywords<News, NewsKeyword>(keywords, news, new List<Keyword> { keyword });
            KeywordHelpers.SetKeywords<Faq, FaqKeyword>(keywords, faq, new List<Keyword> { keyword });

            // Assert
            Assert.Equal(0, news.NewsKeywords.Single().NewsId);
            Assert.Equal(0, faq.FaqKeywords.Single().FaqId);
            Assert.Equal(65, news.NewsKeywords.Single().Keyword.Id);
            Assert.Equal(65, faq.FaqKeywords.Single().Keyword.Id);
        }

        [Fact]
        public void Attach_Old_Relationship()
        {
            // Assign
            var keywords = new[] { "Test" };

            var faq = new Faq { Id = 77 };
            var news = new News { Id = 33 };

            var faqKeyword = new FaqKeyword { FaqId = 77, Faq = faq, KeywordId = 65 };
            var newsKeyword = new NewsKeyword { NewsId = 33, News = news, KeywordId = 65 };

            var keyword = new Keyword
            {
                Id = 65,
                Name = "Test",
                NewsKeywords = new List<NewsKeyword> { newsKeyword },
                FaqKeywords = new List<FaqKeyword> { faqKeyword },
            };

            // Act
            KeywordHelpers.SetKeywords<News, NewsKeyword>(keywords, news, new List<Keyword> { keyword });
            KeywordHelpers.SetKeywords<Faq, FaqKeyword>(keywords, faq, new List<Keyword> { keyword });

            // Assert
            Assert.Equal(33, news.NewsKeywords.Single().NewsId);
            Assert.Equal(77, faq.FaqKeywords.Single().FaqId);
            Assert.Equal(65, faq.FaqKeywords.Single().KeywordId);
            Assert.Equal(65, news.NewsKeywords.Single().KeywordId);
        }

        [Fact]
        public void Attach_Old_Relationship_And_Create_New_Keywords()
        {
            // Assign
            var keywords = new[] { "Test", "Test2" };

            var faq = new Faq { Id = 77 };
            var news = new News { Id = 33 };

            var faqKeyword = new FaqKeyword { FaqId = 77, Faq = faq, KeywordId = 65 };
            var newsKeyword = new NewsKeyword { NewsId = 33, News = news, KeywordId = 65 };

            var keyword = new Keyword
            {
                Id = 65,
                Name = "Test",
                NewsKeywords = new List<NewsKeyword> { newsKeyword },
                FaqKeywords = new List<FaqKeyword> { faqKeyword },
            };

            faqKeyword.Keyword = keyword;
            newsKeyword.Keyword = keyword;

            // Act
            KeywordHelpers.SetKeywords<News, NewsKeyword>(keywords, news, new List<Keyword> { keyword });
            KeywordHelpers.SetKeywords<Faq, FaqKeyword>(keywords, faq, new List<Keyword> { keyword });

            // Assert
            Assert.Equal(33, news.NewsKeywords.Single(nk => nk.Keyword.Name == "Test").NewsId);
            Assert.Equal(77, faq.FaqKeywords.Single(nk => nk.Keyword.Name == "Test").FaqId);
            Assert.Equal(65, faq.FaqKeywords.Single(nk => nk.Keyword.Name == "Test").KeywordId);
            Assert.Equal(65, news.NewsKeywords.Single(nk => nk.Keyword.Name == "Test").KeywordId);
            Assert.Equal(0, faq.FaqKeywords.Single(nk => nk.Keyword.Name == "Test2").Keyword.Id);
            Assert.Equal(0, news.NewsKeywords.Single(nk => nk.Keyword.Name == "Test2").Keyword.Id);
        }
    }
}
