using Intranet.API.Domain.Data;
using Intranet.API.Domain.Models.Entities;
using Intranet.Shared.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Intranet.API.Helpers
{
    /// <summary>
    /// Static Helper Class for working with Keywords and News
    /// </summary>
    public static class NewsKeywordHelper
    {
        // TODO: Add tests
        /// <summary>
        /// Adds keywords to <paramref name="news"/> based on <paramref name="keywordsAsOneString"/>. Will split on ',' and ';'.
        /// </summary>
        /// <param name="keywordsAsOneString"></param>
        /// <param name="news"></param>
        /// <param name="context"></param>
        public static void SetKeywords(string keywordsAsOneString, News news, IntranetApiContext context)
        {
            if (keywordsAsOneString.IsNull())
            {
                return;
            }

            var keywords = keywordsAsOneString?
                .Split(',', ';')?
                .Distinct()?
                .Where(k => !String.IsNullOrWhiteSpace(k))?
                .Select(k => k.Trim());

            var allKeywordEntities = context.Keywords?
                .Include(k => k.NewsKeyword)
                    .ThenInclude(nk => nk.News)?
                .Where(k => keywords.Contains(k.Name, StringComparer.OrdinalIgnoreCase) || k.NewsKeyword.Any(nk => nk.NewsId.Equals(news.Id)))
                .ToList();

            var exisitingKeywordEntities = allKeywordEntities?
                .Where(k => keywords.Contains(k.Name, StringComparer.OrdinalIgnoreCase));

            var alreadyAttachedKeywordEntities = exisitingKeywordEntities
                .Where(k => k.NewsKeyword.Any(nk => nk.News.Id == news.Id));

            var alreadyAttachedNewsKeywordEntities = exisitingKeywordEntities
                .SelectMany(k => k.NewsKeyword)
                .Where(nk => nk.News.Id == news.Id);

            var existingKeywords = exisitingKeywordEntities
                .Select(k => k.Name);

            var newKeywords = keywords?.Except(existingKeywords);

            var newKeywordEntities = newKeywords?
                .Select(name => new Keyword { Name = name });

            var allKeywordEntitiesToBeAttached = exisitingKeywordEntities?
                .Except(alreadyAttachedKeywordEntities)
                .Concat(newKeywordEntities ?? new List<Keyword>());

            var newsKeywordEntities = allKeywordEntitiesToBeAttached?
                .Select(k => new NewsKeyword { Keyword = k })
                .Concat(alreadyAttachedNewsKeywordEntities);

            news.NewsKeywords = newsKeywordEntities?.ToList();
        }
    }
}
