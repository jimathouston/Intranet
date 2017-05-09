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
    }

    public virtual DbSet<News> News { get; set; }
    public virtual DbSet<Employee> Employees { get; set; }
    public virtual DbSet<Checklist> Checklist { get; set; }
    public virtual DbSet<ToDo> ToDos { get; set; }
  }
}
