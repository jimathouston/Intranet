using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Intranet.API.Domain.Models.Entities;
using System.Linq;

namespace Intranet.API.Domain.Data
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

            #region NewsKeyword
            modelBuilder.Entity<NewsKeyword>()
                .HasKey(k => new { k.NewsId, k.KeywordId });

            modelBuilder.Entity<NewsKeyword>()
                .HasOne(nk => nk.News)
                .WithMany(n => n.NewsKeywords)
                .HasForeignKey(nk => nk.NewsId);

            modelBuilder.Entity<NewsKeyword>()
                .HasOne(nk => nk.Keyword)
                .WithMany(k => k.NewsKeywords)
                .HasForeignKey(nk => nk.KeywordId);
            #endregion

            #region Faq
            modelBuilder.Entity<Faq>()
                .HasIndex(f => f.Url)
                .IsUnique();
            #endregion

            #region FaqKeyword
            modelBuilder.Entity<FaqKeyword>()
                .HasKey(k => new { k.FaqId, k.KeywordId });

            modelBuilder.Entity<FaqKeyword>()
                .HasOne(fk => fk.Faq)
                .WithMany(f => f.FaqKeywords)
                .HasForeignKey(fk => fk.FaqId);

            modelBuilder.Entity<FaqKeyword>()
                .HasOne(fk => fk.Keyword)
                .WithMany(k => k.FaqKeywords)
                .HasForeignKey(fk => fk.KeywordId);
            #endregion

            #region Category
            modelBuilder.Entity<Category>()
                .HasIndex(c => c.Url)
                .IsUnique();

            modelBuilder.Entity<Category>()
                .HasIndex(c => c.Title)
                .IsUnique();
            #endregion

            #region Keyword
            modelBuilder.Entity<Keyword>()
                .HasIndex(k => k.Url)
                .IsUnique();

            modelBuilder.Entity<Keyword>()
                .HasIndex(k => k.Name)
                .IsUnique();
            #endregion

            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<Keyword> Keywords { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Faq> Faqs { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
    }
}
