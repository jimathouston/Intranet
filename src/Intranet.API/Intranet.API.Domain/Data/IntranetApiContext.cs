using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Intranet.API.Domain.Models.Entities;
using System.Linq;

namespace Intranet.API.Domain.Data
{
    public class IntranetApiContext : DbContext
    {
        public IntranetApiContext(DbContextOptions<IntranetApiContext> options)
            : base(options)
        {
            // Empty
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                entity.Relational().TableName = entity.ClrType.Name;
            }

            modelBuilder.Entity<EmployeeToDo>()
                .HasKey(t => new { t.ToDoId, t.EmployeeId });

            modelBuilder.Entity<Skill>()
                .HasKey(s => new { s.EmployeeId, s.SkillTypeId });

            modelBuilder.Entity<ProjectEmployee>()
                .HasKey(p => new { p.EmployeeId, p.ProjectId });

            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<ToDo> ToDos { get; set; }
        public virtual DbSet<EmployeeToDo> EmployeeToDos { get; set; }
        public virtual DbSet<Skill> Skills { get; set; }
        public virtual DbSet<SkillType> SkillTypes { get; set; }
        public virtual DbSet<SkillLevel> SkillLevels { get; set; }
        public virtual DbSet<ProjectEmployee> ProjectEmployees { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
    }
}
