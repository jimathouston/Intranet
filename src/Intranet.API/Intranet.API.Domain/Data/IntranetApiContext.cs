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
    public IntranetApiContext()
    {
      // Empty
    }

    public IntranetApiContext(DbContextOptions<IntranetApiContext> options)
      : base(options)
    {
      // Empty
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<ToDo>()
        .HasKey(t => new { t.ChecklistId, t.EmployeeId});

      modelBuilder.Entity<Skill>()
        .HasKey(s => new { s.EmployeeId, s.SkillTypeId});

      modelBuilder.Entity<Assignment>()
        .HasKey(a => new { a.EmployeeId });
    }

    public virtual DbSet<News> News { get; set; }
    public virtual DbSet<Employee> Employees { get; set; }
    public virtual DbSet<Checklist> Checklist { get; set; }
    public virtual DbSet<ToDo> ToDos { get; set; }
    public virtual DbSet<Skill> Skills { get; set; }
    public virtual DbSet<SkillType> SkillTypes { get; set; }
    public virtual DbSet<SkillLevel> SkillLevels { get; set; }
    public virtual DbSet<Assignment> Assignments { get; set; }
    public virtual DbSet<Client> Clients { get; set; }
    public virtual DbSet<Project> Projects { get; set; }
    public virtual DbSet<Location> Locations { get; set; }
    public virtual DbSet<Role> Roles { get; set; }
  }
}
