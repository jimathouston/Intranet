using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Intranet.API.Domain.Data;
using Intranet.API.Data;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;

namespace Intranet.API
{
  public class StartupProduction
  {
    public StartupProduction(IHostingEnvironment env)
    {
      var builder = CommonStartupConfigurations.BuildConfig(env);

      Configuration = builder.Build();
    }

    public IConfigurationRoot Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      var sqlConnectionString = Environment.GetEnvironmentVariable("DATABASE_CONNECTION");
      
      services.AddDbContext<IntranetApiContext>(options =>
        options.UseNpgsql(
          sqlConnectionString,
          b => b.MigrationsAssembly("Intranet.API")   
        )
      );
      
      // Add framework services.
      services.AddMvc(config =>
      {
        // Add authentication everywhere
        var policy = new AuthorizationPolicyBuilder()
          .RequireAuthenticatedUser()
          .Build();

        config.Filters.Add(new AuthorizeFilter(policy));
      });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
    {
      loggerFactory.AddConsole(Configuration.GetSection("Logging"));
      loggerFactory.AddDebug();

      var secretKey = Configuration["INTRANET_JWT"];
      var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));

      var tokenValidationParameters = CommonStartupConfigurations.GetTokenValidationParameters(signingKey);
      var jwtBearerOptions = CommonStartupConfigurations.GetJwtBearerOptions(tokenValidationParameters);

      app.UseJwtBearerAuthentication(jwtBearerOptions);

      app.UseMvc();
    }
  }
}
