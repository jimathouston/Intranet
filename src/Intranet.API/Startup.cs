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
using Swashbuckle.AspNetCore.Swagger;

namespace Intranet.API
{
  public class Startup
  {
    public Startup(IHostingEnvironment env)
    {
      var builder = new ConfigurationBuilder()
          .SetBasePath(env.ContentRootPath)
          .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
          .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
          .AddEnvironmentVariables();

      Configuration = builder.Build();
    }

    public IConfigurationRoot Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      var pathToDoc = ".xml";

      // Add framework services.
      services.AddMvc(config =>
      {
        // Add authentication everywhere
        var policy = new AuthorizationPolicyBuilder()
          .RequireAuthenticatedUser()
          .Build();

        config.Filters.Add(new AuthorizeFilter(policy));
      });

      // Add Swagger Generator
      services.AddSwaggerGen(options =>
      {
        options.SwaggerDoc("v1",
          new Info
          {
            Title = "Certaincy Intranet",
            Version = "v1",
            Description = "Documentation for Certaincy Intranet API. Only available on the development enviroment.",
          });

        var filePath = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, pathToDoc);
        options.IncludeXmlComments(filePath);
        options.DescribeAllEnumsAsStrings();

        options.AddSecurityDefinition("Bearer", new ApiKeyScheme()
        {
          Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
          Name = "Authorization",
          In = "header",
          Type = "apiKey",
        });
      });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
    {
      loggerFactory.AddConsole(Configuration.GetSection("Logging"));
      loggerFactory.AddDebug();

      var secretKey = Configuration["INTRANET_JWT"];
      var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));

      var tokenValidationParameters = new TokenValidationParameters
      {
        // The signing key must match!
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = signingKey,

        // Validate the JWT Issuer (iss) claim
        ValidateIssuer = true,
        ValidIssuer = "ExampleIssuer",

        // Validate the JWT Audience (aud) claim
        ValidateAudience = true,
        ValidAudience = "ExampleAudience",

        // Validate the token expiry
        ValidateLifetime = true,

        // If you want to allow a certain amount of clock drift, set that here:
        ClockSkew = TimeSpan.Zero,
      };

      app.UseJwtBearerAuthentication(new JwtBearerOptions
      {
        AutomaticAuthenticate = true,
        AutomaticChallenge = true,
        TokenValidationParameters = tokenValidationParameters
      });

      if (env.IsDevelopment())
      {
        app.UseSwagger(c => 
        {
          c.RouteTemplate = "api-docs/swagger/{documentName}/swagger.json";
        });

        app.UseSwaggerUI(c =>
        {
          c.RoutePrefix = "api-docs";
          c.SwaggerEndpoint("/api-docs/swagger/v1/swagger.json", "v1 docs");
        });
      }

      app.UseMvc();
    }
  }
}
