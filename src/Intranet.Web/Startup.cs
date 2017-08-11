using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Amazon.S3;
using Intranet.Web.Authentication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Intranet.Web.Authentication.Providers;
using Intranet.Web.Authentication.Services;
using Intranet.Web.Common.Factories;
using Intranet.Web.Common.Models.Options;
using Intranet.Web.Domain.Data;
using Newtonsoft.Json;
using Intranet.Web.Filters;
using Intranet.Web.Extensions;
using Intranet.Services.ImageService;
using Intranet.Services.FileStorageService;
using Amazon.S3.Transfer;
using Intranet.Web.Models.Options;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.AspNetCore.ResponseCompression;
using Intranet.Web.Authentication.Services.E2E;

namespace Intranet.Web
{
    public class Startup
    {
        private readonly IHostingEnvironment CurrentEnvironment;

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

            if (env.IsDevelopment())
            {
                // Bodge to be able to use user-secrets because... aws... 🤦
                Environment.SetEnvironmentVariable("AWS_BUCKET_NAME", Configuration["AWS_BUCKET_NAME"]);
                Environment.SetEnvironmentVariable("AWS_SECRET_ACCESS_KEY", Configuration["AWS_SECRET_ACCESS_KEY"]);
                Environment.SetEnvironmentVariable("AWS_REGION", Configuration["AWS_REGION"]);
                Environment.SetEnvironmentVariable("AWS_ACCESS_KEY_ID", Configuration["AWS_ACCESS_KEY_ID"]);
            }
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region Dependency Injection
            if (CurrentEnvironment.IsProduction())
            {
                services.AddTransient<IAuthenticationProvider, AuthenticationProvider>();
            }
            else
            {
                services.AddTransient<IAuthenticationProvider, DevelopmentAuthenticationProvider>();
            }

            if (CurrentEnvironment.IsE2e())
            {
                services.AddTransient<IAuthenticationService, E2EAuthenticationService>();
            }
            else
            {
                services.AddTransient<IAuthenticationService, LdapAuthenticationService>();
            }

            services.AddTransient<IDateTimeFactory, DateTimeFactory>();
            services.AddTransient<IImageService, ImageService>();
            services.AddAWSService<IAmazonS3>();
            services.AddTransient<ITransferUtility, TransferUtility>();
            services.AddTransient<IFileStorageService, S3FileStorageService>();
            #endregion

            #region Database
            var sqlConnectionString = Configuration["CONNECTION_STRING"];

            services.AddDbContext<IntranetApiContext>(opt => opt.UseSqlServer(sqlConnectionString));
            #endregion

            #region Options
            // Required to use the Options<T> pattern
            services.AddOptions();

            // Add settings from configuration
            services.Configure<LdapConfig>(options =>
            {
                options.AdminCn = Configuration["LDAP_ADMIN_CN"];
                options.DeveloperCn = Configuration["LDAP_DEVELOPER_CN"];
                options.BindCredentials = Configuration["LDAP_BIND_CREDENTIALS"];
                options.BindDn = Configuration["LDAP_BIND_DN"];
                options.SearchBase = Configuration["LDAP_SEARCH_BASE"];
                options.SearchFilter = Configuration["LDAP_SEARCH_FILTER"];
                options.Url = Configuration["LDAP_URL"];
            });

            services.Configure<S3Options>(options =>
            {
                options.BucketName = Configuration["AWS_BUCKET_NAME"];
            });

            services.Configure<GoogleAnalyticsOptions>(options =>
            {
                options.TrackingId = Configuration["GA_TRACKING_ID"];
            });

            services.Configure<GzipCompressionProviderOptions>(options =>
                options.Level = System.IO.Compression.CompressionLevel.Optimal
            );
            #endregion

            // TODO: Enforce SSL: https://docs.microsoft.com/en-us/aspnet/core/security/enforcing-ssl

            #region Response Compression
            services.AddResponseCompression();
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
                //config.Filters.Add(new ModelValidationFilterAttribute());
            })
            .AddJsonOptions(opt =>
            {
                // From: http://stackoverflow.com/questions/41728737/iso-utc-datetime-format-as-default-json-output-format-in-mvc-6-api-response
                opt.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });
            #endregion

            #region Custom Authorization Policies
            services.AddAuthorization(options =>
            {
                options.AddPolicy(
                    "IsAdmin",
                    policyBuilder => policyBuilder.RequireAssertion(
                        context => context.User.IsAdmin())
                    );
            });
            #endregion

            #region AWS
            var awsOptions = Configuration.GetAWSOptions();
            services.AddDefaultAWSOptions(awsOptions);
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IntranetApiContext dbContext)
        {
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

            #region Cookie Authentication
            // Add Authentication
            // TODO: Added validation of user and expiration: https://docs.microsoft.com/en-us/aspnet/core/security/authentication/cookie#reacting-to-back-end-changes
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationScheme = "Cookies",
                LoginPath = new PathString("/Login"),
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
            });
            #endregion

            #region Logging
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            #endregion

            #region Response Compression
            app.UseResponseCompression();
            #endregion

            #region Static Files
            app.UseProtectFolder(new ProtectFolderOptions
            {
                Path = "/Assets",
            });

            app.UseStaticFiles(); // For the wwwroot folder

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), @"Assets")
                ),
                RequestPath = new PathString("/Assets")
            });
            #endregion

            #region Mvc
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=News}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "auth",
                    template: "{action=Index}",
                    defaults: new { controller = "Authentication" });
            });
            #endregion
        }
    }
}
