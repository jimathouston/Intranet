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
using Intranet.API.Data;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;
using Intranet.API.Domain;
using Intranet.API.Filters;

namespace Intranet.API
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = CommonStartupConfigurations.BuildConfig(env);

            Configuration = builder.Build();
            CurrentEnvironment = env;
        }

        public IConfigurationRoot Configuration { get; }
        public IHostingEnvironment CurrentEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region Database
            if (CurrentEnvironment.IsProduction() || CurrentEnvironment.IsStaging())
            {
                var sqlConnectionString = Environment.GetEnvironmentVariable("DATABASE_CONNECTION");

                services.ConfigureDbForProduction<IntranetApiContext>(sqlConnectionString);
            }
            else if (CurrentEnvironment.IsDevelopment())
            {
                services.ConfigureDbForDevelopment<IntranetApiContext>();
            }
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
            #endregion

            #region CORS
            if (CurrentEnvironment.IsDevelopment())
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
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            #region Logger
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            #endregion

            #region Authentication
            var secretKey = Configuration["INTRANET_JWT"];
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));

            var tokenValidationParameters = CommonStartupConfigurations.GetTokenValidationParameters(signingKey);
            var jwtBearerOptions = CommonStartupConfigurations.GetJwtBearerOptions(tokenValidationParameters);

            app.UseJwtBearerAuthentication(jwtBearerOptions);
            #endregion

            #region Swagger
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "api-docs/swagger/{documentName}/swagger.json";
            });

            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "api-docs";
                c.SwaggerEndpoint("/api-docs/swagger/v1/swagger.json", "v1 docs");
            });
            #endregion

            #region CORS
            if (env.IsDevelopment())
            {
                app.UseCors("CorsPolicy");
            }
            #endregion

            #region Mvc
            app.UseMvc();
            #endregion

            #region Seed database
            if (env.IsDevelopment())
            {
                var context = app.ApplicationServices.GetService<IntranetApiContext>();
                DbInitializer.SeedDb(context);
            }
            #endregion
        }
    }
}
