using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Intranet.Web.Domain.Models.Entities;
using System.Linq;

namespace Intranet.Web.Domain.Data
{
    /// <summary>
    /// Configuration of database
    /// </summary>
    public class IntranetApiContext : DbContext
    {
        public IntranetApiContext(DbContextOptions<IntranetApiContext> options)
            : base(options)
        {
            // Empty
        }

        /// <summary>
        /// Setup of entities
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Use the Class name as Table name
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                entity.Relational().TableName = entity.ClrType.Name;
            }

            #region News
            modelBuilder.Entity<News>()
                .Property(n => n.HasEverBeenPublished)
                .UsePropertyAccessMode(Microsoft.EntityFrameworkCore.Metadata.PropertyAccessMode.FieldDuringConstruction);

            modelBuilder.Entity<News>()
                .Property(n => n.Published)
                .UsePropertyAccessMode(Microsoft.EntityFrameworkCore.Metadata.PropertyAccessMode.Property);

            modelBuilder.Entity<News>()
                .HasIndex(n => n.Url);
            #endregion

            #region NewsTag
            modelBuilder.Entity<NewsTag>()
                .HasKey(k => new { k.NewsId, k.TagId });

            modelBuilder.Entity<NewsTag>()
                .HasOne(nt => nt.News)
                .WithMany(n => n.NewsTags)
                .HasForeignKey(nt => nt.NewsId);

            modelBuilder.Entity<NewsTag>()
                .HasOne(nt => nt.Tag)
                .WithMany(k => k.NewsTags)
                .HasForeignKey(nt => nt.TagId);
            #endregion

            #region Faq
            modelBuilder.Entity<Faq>()
                .HasIndex(f => f.Url)
                .IsUnique();
            #endregion

            #region FaqTag
            modelBuilder.Entity<FaqTag>()
                .HasKey(k => new { k.FaqId, k.TagId });

            modelBuilder.Entity<FaqTag>()
                .HasOne(ft => ft.Faq)
                .WithMany(f => f.FaqTags)
                .HasForeignKey(ft => ft.FaqId);

            modelBuilder.Entity<FaqTag>()
                .HasOne(ft => ft.Tag)
                .WithMany(k => k.FaqTags)
                .HasForeignKey(ft => ft.TagId);
            #endregion

            #region Policy
            modelBuilder.Entity<Policy>()
                .HasIndex(p => p.Url)
                .IsUnique();
            #endregion

            #region PolicyTag
            modelBuilder.Entity<PolicyTag>()
                .HasKey(k => new { k.PolicyId, k.TagId });

            modelBuilder.Entity<PolicyTag>()
                .HasOne(pt => pt.Policy)
                .WithMany(p => p.PolicyTags)
                .HasForeignKey(pt => pt.PolicyId);

            modelBuilder.Entity<PolicyTag>()
                .HasOne(pt => pt.Tag)
                .WithMany(k => k.PolicyTags)
                .HasForeignKey(pt => pt.TagId);
            #endregion

            #region Category
            modelBuilder.Entity<Category>()
                .HasIndex(c => c.Url)
                .IsUnique();

            modelBuilder.Entity<Category>()
                .HasIndex(c => c.Title)
                .IsUnique();
            #endregion

            #region Tag
            modelBuilder.Entity<Tag>()
                .HasIndex(k => k.Url)
                .IsUnique();

            modelBuilder.Entity<Tag>()
                .HasIndex(k => k.Name)
                .IsUnique();
            #endregion

            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Faq> Faqs { get; set; }
        public virtual DbSet<Policy> Policies { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
    }
}
