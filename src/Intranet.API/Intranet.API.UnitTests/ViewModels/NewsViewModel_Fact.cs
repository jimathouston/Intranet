using Intranet.API.Domain.Models.Entities;
using Intranet.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Intranet.API.UnitTests.ViewModels
{
    public class NewsViewModel_Fact
    {
        [Fact]
        public void If_Null_Return_Null()
        {
            // Assign
            var newsViewModel = new NewsViewModel();

            newsViewModel.NewsKeywords = null;
            // Act

            // Assert
            Assert.Null(newsViewModel.Keywords);
        }

        [Fact]
        public void Return_Keyword()
        {
            // Assign
            var newsViewModel = new NewsViewModel();

            newsViewModel.NewsKeywords = new List<NewsKeyword>
            {
                new NewsKeyword
                {
                    Keyword = new Keyword
                    {
                        Name = "test",
                    }
                }
            };
            // Act

            // Assert
            Assert.Equal(newsViewModel.Keywords, "test");
        }

        [Fact]
        public void Return_Keywords()
        {
            // Assign
            var newsViewModel = new NewsViewModel();

            newsViewModel.NewsKeywords = new List<NewsKeyword>
            {
                new NewsKeyword
                {
                    Keyword = new Keyword
                    {
                        Name = "test",
                    }
                },
                new NewsKeyword
                {
                    Keyword = new Keyword
                    {
                        Name = "test2",
                    }
                },
            };
            // Act

            // Assert
            Assert.Equal(newsViewModel.Keywords, "test,test2");
        }

        [Fact]
        public void Return_Keywords_From_News()
        {
            // Assign
            var news = new News
            {
                NewsKeywords = new List<NewsKeyword>
                {
                    new NewsKeyword
                    {
                        Keyword = new Keyword
                        {
                            Name = "test",
                        }
                    },
                    new NewsKeyword
                    {
                        Keyword = new Keyword
                        {
                            Name = "test2",
                        }
                    },
                },
            };

            var newsViewModel = new NewsViewModel(news);
            // Act

            // Assert
            Assert.Equal(newsViewModel.Keywords, "test,test2");
        }

        [Fact]
        public void Should_Be_Possible_To_Set_Keywords()
        {
            // Assign
            var newsViewModel = new NewsViewModel();

            // Act
            newsViewModel.Keywords = "test";

            // Assert
            Assert.Equal(newsViewModel.Keywords, "test");
        }
    }
}
