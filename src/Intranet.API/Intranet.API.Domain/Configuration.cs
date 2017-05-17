using Intranet.API.Domain.Data;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;


namespace Intranet.API.Domain
{
  public static class Configuration
  {
    public static void ConfigureDb<TContext>(this IServiceCollection services)
      where TContext : DbContext
    {
      services.AddDbContext<TContext>(opt => opt.UseInMemoryDatabase());
    }
  }
}
