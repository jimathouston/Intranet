using Intranet.API.Domain.Data;
using Intranet.API.Domain.Models.Entities;
using Intranet.Shared.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intranet.API.Domain.Helpers
{
    public static class KeywordHelpers
    {
        public static async Task<bool> HasNoRelatedEntities(Keyword entity, IntranetApiContext context, IEnumerable<object> ignore = null)
        {
            // Eager load all navigation properties
            await context
                .Entry(entity)
                .Collection(c => c.FaqKeywords)
                .LoadAsync();

            await context
                .Entry(entity)
                .Collection(c => c.NewsKeywords)
                .LoadAsync();

            await context
                .Entry(entity)
                .Collection(c => c.PolicyKeywords)
                .LoadAsync();

            if (ignore.IsNotNull())
            {
                return
                    ((entity.FaqKeywords.Intersect(ignore).Any() && entity.FaqKeywords.Count == 1) || entity.FaqKeywords.Count == 0)
                    && ((entity.NewsKeywords.Intersect(ignore).Any() && entity.NewsKeywords.Count == 1) || entity.NewsKeywords.Count == 0)
                    && ((entity.PolicyKeywords.Intersect(ignore).Any() && entity.PolicyKeywords.Count == 1) || entity.PolicyKeywords.Count == 0);
            }

            return
                entity.NewsKeywords.Count == 0
                && entity.FaqKeywords.Count == 0
                && entity.PolicyKeywords.Count == 0;
        }
    }
}
