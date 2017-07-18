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
using Intranet.API.Domain.Data;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;
using Intranet.API.Domain;
using Intranet.API.Filters;
using Microsoft.EntityFrameworkCore;

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

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }

            Configuration = builder.Build();
            CurrentEnvironment = env;
        }

        public IConfigurationRoot Configuration { get; }
        public IHostingEnvironment CurrentEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region Database
            var sqlConnectionString = Configuration["CONNECTION_STRING"];

            services.AddDbContext<IntranetApiContext>(opt => opt.UseSqlServer(sqlConnectionString));
            #endregion

            #region Mvc
            // Add framework services.
            services.AddMvc(config =>
            {
                // Add authentication everywhere
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();

                config.Filters.Add(new AuthorizeFilter(policy));
                config.Filters.Add(new ModelValidationFilterAttribute());
            })
            .AddJsonOptions(opt =>
            {
                // From: http://stackoverflow.com/questions/41728737/iso-utc-datetime-format-as-default-json-output-format-in-mvc-6-api-response
                opt.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });
            #endregion

            #region Swagger
            if (CurrentEnvironment.IsDevelopment())
            {
                // Add Swagger Generator
                var pathToDoc = ".xml";

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
            #endregion

            #region CORS
            if (!CurrentEnvironment.IsProduction())
            {
                services.AddCors(options =>
                {
                    options.AddPolicy("CorsPolicy",
                        builder => builder.AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials());
                });
            }
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IntranetApiContext dbContext)
        {
            #region Logger
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            #endregion

            #region Migrate Database
            try
            {
                dbContext.Database.Migrate();
            }
            catch (Exception exception)
            {
                // TODO: Add logging, notification etc.
                Console.WriteLine(exception.Message);
                Environment.Exit(1);
            }
            #endregion

            #region Authentication
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

            var jwtBearerOptions = new JwtBearerOptions
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                TokenValidationParameters = tokenValidationParameters
            };

            app.UseJwtBearerAuthentication(jwtBearerOptions);
            #endregion

            #region Swagger
            if (env.IsDevelopment())
            {
                app.UseSwagger(c =>
                {
                    c.RouteTemplate = "api-docs/swagger/{documentName}/swagger.json";
                });
            }
            #endregion

            #region CORS
            if (!env.IsProduction())
            {
                app.UseCors("CorsPolicy");
            }
            #endregion

            #region Mvc
            app.UseMvc();
            #endregion
        }
    }
}
