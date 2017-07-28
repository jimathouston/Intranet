using Intranet.API.Domain.Models.Entities;
using Intranet.API.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Intranet.API.UnitTests.Helpers
{
    public class KeywordHelper_Fact
    {
        [Fact]
        public void Dont_Throw_On_Null()
        {
            // Assign

            // Act
            KeywordHelper.SetKeywords<News, NewsKeyword>(null, null, null);
            KeywordHelper.GetKeywordsFromString(null);

            // Assert
        }

        [Fact]
        public void Return_Array()
        {
            // Assign
            var keywordsInput = "Test, test2; test-again";

            // Act
            var keywords = KeywordHelper.GetKeywordsFromString(keywordsInput);

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
            KeywordHelper.SetKeywords<News, NewsKeyword>(keywords, news, null);
            KeywordHelper.SetKeywords<Faq, FaqKeyword>(keywords, faq, null);

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
            KeywordHelper.SetKeywords<News, NewsKeyword>(keywords, news, new List<Keyword> { keyword });
            KeywordHelper.SetKeywords<Faq, FaqKeyword>(keywords, faq, new List<Keyword> { keyword });

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
            KeywordHelper.SetKeywords<News, NewsKeyword>(keywords, news, new List<Keyword> { keyword });
            KeywordHelper.SetKeywords<Faq, FaqKeyword>(keywords, faq, new List<Keyword> { keyword });

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
            KeywordHelper.SetKeywords<News, NewsKeyword>(keywords, news, new List<Keyword> { keyword });
            KeywordHelper.SetKeywords<Faq, FaqKeyword>(keywords, faq, new List<Keyword> { keyword });

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
