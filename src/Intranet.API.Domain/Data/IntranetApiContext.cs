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
    }

    public virtual DbSet<News> News { get; set; }
  }
}
