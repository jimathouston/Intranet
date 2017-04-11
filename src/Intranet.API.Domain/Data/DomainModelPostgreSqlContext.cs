using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Intranet.API.Domain.Models.Entities;
using System.Linq;

namespace Intranet.API.Domain.Data
{
  // Connection to Db. Source: https://damienbod.com/2016/01/11/asp-net-5-with-postgresql-and-entity-framework-7/
  public class DomainModelPostgreSqlContext : DbContext
  {
    public DomainModelPostgreSqlContext(DbContextOptions<DomainModelPostgreSqlContext> options) : base(options)
    {
    }

    public DbSet<News> News { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      builder.Entity<News>().HasKey(m => m.Id);

      base.OnModelCreating(builder);
    }

    public override int SaveChanges()
    {
      ChangeTracker.DetectChanges();
      updateUpdatedProperty<News>();

      return base.SaveChanges();
    }

    private void updateUpdatedProperty<T>() where T : class
    {
      var modifiedSourceInfo =
          ChangeTracker.Entries<T>()
              .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);
    }
  }
}
