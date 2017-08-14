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
    public static class TagHelpers
    {
        // TODO: Add tests
        /// <summary>
        /// Adds tags to <paramref name="entity"/> based on <paramref name="tags"/>.
        /// </summary>
        /// <param name="tags"></param>
        /// <param name="entity"></param>
        /// <param name="allTagEntities"></param>
        public static void SetTags<TEntity, TRelation>(IEnumerable<string> tags, TEntity entity, ICollection<Tag> allTagEntities)
            where TEntity : IHasTags
            where TRelation : class, ITagRelation
        {
            if (tags.IsNull())
            {
                return;
            }

            var nameOfEntity = entity.GetType().Name;
            var nameOfJoiningProperty = $"{nameOfEntity}{nameof(Tag)}s"; // ie NewsTags

            var joiningTableType = typeof(Tag).GetProperty(nameOfJoiningProperty).PropertyType.GetGenericArguments()[0]; // ie NewsTag

            // Get all tag entities that already is created
            var exisitingTagEntities = allTagEntities?
                .Where(k => tags.Contains(k.Name, StringComparer.OrdinalIgnoreCase))
                ?? new List<Tag>();

            // Get a list of all tags that is already attached to the entity to avoid duplicates etc
            var alreadyAttachedTagEntities = exisitingTagEntities?
                .Where(k =>
                    GetJoiningCollectionInternal<TRelation>(k, nameOfJoiningProperty)?
                        .Any(jt => GetEntityInternal<TEntity>(jt, nameOfEntity).Id == entity.Id) == true
                );

            // Get all joining tables, ie NewsTag, that is already attached
            var alreadyAttachedJoiningTagEntities = exisitingTagEntities?
                .SelectMany(k => GetJoiningCollectionInternal<TRelation>(k, nameOfJoiningProperty))?
                .Where(jt => GetEntityInternal<TEntity>(jt, nameOfEntity).Id == entity.Id) ?? new List<TRelation>();

            // Get all existing tags as strings compare them to the param to avoid creating them again
            var existingTags = exisitingTagEntities?
                .Select(k => k.Name) ?? new List<string>();

            // Finally we have a list of all tags we have to create
            var newTags = tags?.Except(existingTags);

            // Create entities
            var newTagEntities = newTags?
                .Select(name => new Tag
                {
                    Name = name,
                    Url = CustomUrlHelper.URLFriendly(name),
                });

            // Get a list of all tag entities that have be attached to the entity
            var allTagEntitiesToBeAttached = exisitingTagEntities
                .Except(alreadyAttachedTagEntities)
                .Concat(newTagEntities ?? new List<Tag>());

            // Create new relations between the tag entities and the entity (many-to-many)
            var joiningTagEntities = allTagEntitiesToBeAttached?
                .Select(k => Activator.CreateInstance(joiningTableType, k))?
                // Add all old entities that is already attached (meaning there already exists entities for them)
                .Concat(alreadyAttachedJoiningTagEntities)?
                .Select(k => k as TRelation)?
                .ToList();

            // Set the new list of relationship entities to the correct property of the entity
            entity.GetType().GetProperty(nameOfJoiningProperty).SetValue(entity, joiningTagEntities);
        }

        /// <summary>
        /// Will split <paramref name="tags"/> on ',' and ';'.
        /// </summary>
        /// <param name="tags"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetTagsFromString(string tags)
        {
            return tags?
                .Split(',', ';')?
                .Distinct()?
                .Where(k => !String.IsNullOrWhiteSpace(k))?
                .Select(k => k.Trim());
        }

        public static async Task<bool> HasNoRelatedEntities(Tag entity, IntranetApiContext context, IEnumerable<object> ignore = null)
        {
            // Eager load all navigation properties
            await context
                .Entry(entity)
                .Collection(c => c.FaqTags)
                .LoadAsync();

            await context
                .Entry(entity)
                .Collection(c => c.NewsTags)
                .LoadAsync();

            await context
                .Entry(entity)
                .Collection(c => c.PolicyTags)
                .LoadAsync();

            if (ignore.IsNotNull())
            {
                return
                    ((entity.FaqTags.Intersect(ignore).Any() && entity.FaqTags.Count == 1) || entity.FaqTags.Count == 0)
                    && ((entity.NewsTags.Intersect(ignore).Any() && entity.NewsTags.Count == 1) || entity.NewsTags.Count == 0)
                    && ((entity.PolicyTags.Intersect(ignore).Any() && entity.PolicyTags.Count == 1) || entity.PolicyTags.Count == 0);
            }

            return
                entity.NewsTags.Count == 0
                && entity.FaqTags.Count == 0
                && entity.PolicyTags.Count == 0;
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

        private static ICollection<TRelation> GetJoiningCollectionInternal<TRelation>(Tag entity, string collection)
            where TRelation : class, ITagRelation
        {
            return entity.GetType().GetProperty(collection).GetValue(entity) as ICollection<TRelation> ?? new List<TRelation>();
        }
        #endregion
    }
}
