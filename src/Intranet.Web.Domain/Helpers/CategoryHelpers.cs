using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Intranet.Web.Domain.Data;
using Intranet.Web.Domain.Models.Entities;
using Intranet.Web.Common.Extensions;

namespace Intranet.Web.Domain.Helpers
{
    public static class CategoryHelpers
    {
        public static async Task<bool> HasNoRelatedEntities(Category entity, IntranetApiContext context, object ignore = null)
        {
            // Eager load all navigation properties
            await context
                .Entry(entity)
                .Collection(c => c.Faqs)
                .LoadAsync();

            await context
                .Entry(entity)
                .Collection(c => c.Policies)
                .LoadAsync();

            if (ignore.IsNotNull())
            {
                return
                    ((entity.Faqs.Contains(ignore) && entity.Faqs.Count == 1) || entity.Faqs.Count == 0)
                    && ((entity.Policies.Contains(ignore) && entity.Policies.Count == 1) || entity.Policies.Count == 0);
            }

            return entity.Policies.Count == 0 && entity.Faqs.Count == 0;
        }
    }
}
