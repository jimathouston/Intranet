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

            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<User> Users { get; set; }
    }
}
