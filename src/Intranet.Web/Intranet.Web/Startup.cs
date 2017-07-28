using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Intranet.Shared.Factories;
using Intranet.Web.Models.Options;
using Intranet.Web.Providers;
using Intranet.Web.Providers.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Intranet.Web.Authentication.Models;
using Intranet.Web.Authentication.Services;
using Intranet.Web.Authentication.Providers;
using Intranet.Web.Services;
using Amazon.S3;
using Amazon.S3.Transfer;

namespace Intranet_Web
{
    public class Startup
    {
        private readonly IHostingEnvironment CurrentEnvironment;

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                    .SetBasePath(env.ContentRootPath)
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
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

            services.AddTransient<IAuthenticationService, LdapAuthenticationService>();
            services.AddTransient<ITokenProvider, JwtTokenProvider>();
            services.AddTransient<IDateTimeFactory, DateTimeFactory>();
            services.AddTransient<IImageService, ImageService>();
            services.AddAWSService<IAmazonS3>();
            services.AddTransient<ITransferUtility, TransferUtility>();
            services.AddTransient<IFileStorageService, S3FileStorageService>();
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
            services.Configure<TokenProviderOptions>(options =>
            {
                var secretKey = Configuration["INTRANET_JWT"];
                var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));

                options.Audience = "ExampleAudience";
                options.Issuer = "ExampleIssuer";
                options.SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

                if (CurrentEnvironment.IsDevelopment())
                {
                    // Set exp to 9 hours to be able to reuse the JWT in POSTMAN during the work day.
                    // The secret is different in Development, Staging and Production so the JWT will
                    // only be valid in development and nowhere else.
                    options.Expiration = TimeSpan.FromHours(9);
                }
            });
            services.Configure<S3Options>(options =>
            {
                options.BucketName = Configuration["AWS_BUCKET_NAME"];
            });
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
            });
            #endregion

            #region AWS
            var awsOptions = Configuration.GetAWSOptions();
            services.AddDefaultAWSOptions(awsOptions);
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
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

            #region Webpack
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            #endregion

            #region wwwroot
            app.UseStaticFiles();
            #endregion

            #region Mvc
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                        name: "default",
                        template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "auth",
                    template: "{action=Index}",
                    defaults: new { controller = "Authentication" });

                routes.MapSpaFallbackRoute(
                        name: "spa-fallback",
                        defaults: new { controller = "Home", action = "Index" });
            });
            #endregion
        }
    }
}
