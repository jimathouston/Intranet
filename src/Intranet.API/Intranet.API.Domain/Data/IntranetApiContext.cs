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
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                entity.Relational().TableName = entity.ClrType.Name;
            }

            modelBuilder.Entity<EmployeeToDo>()
                .HasKey(t => new { t.ToDoId, t.EmployeeId });

            modelBuilder.Entity<EmployeeSkill>()
                .HasKey(s => new { s.EmployeeId, s.SkillId });

            modelBuilder.Entity<ProjectEmployee>()
                .HasKey(p => new { p.EmployeeId, p.ProjectId });

            modelBuilder.Entity<NewsTag>()
                .HasKey(t => new { t.NewsId, t.TagId });

            modelBuilder.Entity<NewsTag>()
                .HasOne(pt => pt.News)
                .WithMany(p => p.NewsTags)
                .HasForeignKey(pt => pt.NewsId);

            modelBuilder.Entity<NewsTag>()
                .HasOne(pt => pt.Tag)
                .WithMany(t => t.NewsTags)
                .HasForeignKey(pt => pt.TagId);

            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<ToDo> ToDos { get; set; }
        public virtual DbSet<EmployeeToDo> EmployeeToDos { get; set; }
        public virtual DbSet<EmployeeSkill> EmployeeSkills { get; set; }
        public virtual DbSet<Skill> Skills { get; set; }
        public virtual DbSet<SkillLevel> SkillLevels { get; set; }
        public virtual DbSet<ProjectEmployee> ProjectEmployees { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<NewsTag> NewsTags { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<User> Users { get; set; }
    }
}
