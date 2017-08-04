using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Intranet.Web.Domain.Data;
using Intranet.Web.Domain.Models.Entities;
using Intranet.Web.Common.Extensions;
using System.Reflection;
using Intranet.Web.Domain.Contracts;
using Intranet.Web.Common.Helpers;

namespace Intranet.Web.Domain.Helpers
{
    public static class KeywordHelpers
    {
        // TODO: Add tests
        /// <summary>
        /// Adds keywords to <paramref name="entity"/> based on <paramref name="keywords"/>.
        /// </summary>
        /// <param name="keywords"></param>
        /// <param name="entity"></param>
        /// <param name="allKeywordEntities"></param>
        public static void SetKeywords<TEntity, TRelation>(IEnumerable<string> keywords, TEntity entity, ICollection<Keyword> allKeywordEntities)
            where TEntity : IHasKeywords
            where TRelation : class, IKeywordRelation
        {
            if (keywords.IsNull())
            {
                return;
            }

            var nameOfEntity = entity.GetType().Name;
            var nameOfJoiningProperty = $"{nameOfEntity}{nameof(Keyword)}s"; // ie NewsKeywords

            var joiningTableType = typeof(Keyword).GetProperty(nameOfJoiningProperty).PropertyType.GetGenericArguments()[0]; // ie NewsKeyword

            // Get all keyword entities that already is created
            var exisitingKeywordEntities = allKeywordEntities?
                .Where(k => keywords.Contains(k.Name, StringComparer.OrdinalIgnoreCase))
                ?? new List<Keyword>();

            // Get a list of all keywords that is already attached to the entity to avoid duplicates etc
            var alreadyAttachedKeywordEntities = exisitingKeywordEntities?
                .Where(k =>
                    GetJoiningCollectionInternal<TRelation>(k, nameOfJoiningProperty)?
                        .Any(jt => GetEntityInternal<TEntity>(jt, nameOfEntity).Id == entity.Id) == true
                );

            // Get all joining tables, ie NewsKeyword, that is already attached
            var alreadyAttachedJoiningKeywordEntities = exisitingKeywordEntities?
                .SelectMany(k => GetJoiningCollectionInternal<TRelation>(k, nameOfJoiningProperty))?
                .Where(jt => GetEntityInternal<TEntity>(jt, nameOfEntity).Id == entity.Id) ?? new List<TRelation>();

            // Get all existing keywords as strings compare them to the param to avoid creating them again
            var existingKeywords = exisitingKeywordEntities?
                .Select(k => k.Name) ?? new List<string>();

            // Finally we have a list of all keywords we have to create
            var newKeywords = keywords?.Except(existingKeywords);

            // Create entities
            var newKeywordEntities = newKeywords?
                .Select(name => new Keyword
                {
                    Name = name,
                    Url = CustomUrlHelper.URLFriendly(name),
                });

            // Get a list of all keyword entities that have be attached to the entity
            var allKeywordEntitiesToBeAttached = exisitingKeywordEntities
                .Except(alreadyAttachedKeywordEntities)
                .Concat(newKeywordEntities ?? new List<Keyword>());

            // Create new relations between the keyword entities and the entity (many-to-many)
            var joiningKeywordEntities = allKeywordEntitiesToBeAttached?
                .Select(k => Activator.CreateInstance(joiningTableType, k))?
                // Add all old entities that is already attached (meaning there already exists entities for them)
                .Concat(alreadyAttachedJoiningKeywordEntities)?
                .Select(k => k as TRelation)?
                .ToList();

            // Set the new list of relationship entities to the correct property of the entity
            entity.GetType().GetProperty(nameOfJoiningProperty).SetValue(entity, joiningKeywordEntities);
        }

        /// <summary>
        /// Will split <paramref name="keywords"/> on ',' and ';'.
        /// </summary>
        /// <param name="keywords"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetKeywordsFromString(string keywords)
        {
            return keywords?
                .Split(',', ';')?
                .Distinct()?
                .Where(k => !String.IsNullOrWhiteSpace(k))?
                .Select(k => k.Trim());
        }

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

        #region Private Helpers
        private static int GetIdInternal(object entity)
        {
            return (int)entity.GetType().GetProperty("Id").GetValue(entity);
        }

        private static TEntity GetEntityInternal<TEntity>(object entity, string property)
        {
            return (TEntity)entity.GetType().GetProperty(property).GetValue(entity);
        }

        private static ICollection<TRelation> GetJoiningCollectionInternal<TRelation>(Keyword entity, string collection)
            where TRelation : class, IKeywordRelation
        {
            return entity.GetType().GetProperty(collection).GetValue(entity) as ICollection<TRelation> ?? new List<TRelation>();
        }
        #endregion
    }
}
