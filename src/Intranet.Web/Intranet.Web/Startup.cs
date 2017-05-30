using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Intranet.Web.Factories;
using Intranet.Web.Models.Options;
using Intranet.Web.Providers;
using Intranet.Web.Providers.Contracts;
using Intranet.Web.Services;
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

namespace Intranet_Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                    .SetBasePath(env.ContentRootPath)
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                    .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Dependency injection
            services.AddTransient<IAuthenticationProvider, AuthenticationProvider>();
            services.AddTransient<IAuthenticationService, AuthenticationService>();
            services.AddTransient<ITokenProvider, JwtTokenProvider>();
            services.AddTransient<IDateTimeFactory, DateTimeFactory>();

            // Required to use the Options<T> pattern
            services.AddOptions();

            // Add settings from configuration
            services.Configure<TokenProviderOptions>(options =>
            {
                var secretKey = Configuration["INTRANET_JWT"];
                var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));

                options.Audience = "ExampleAudience";
                options.Issuer = "ExampleIssuer";
                options.SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            });

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
            // Add Authentication
            // TODO: Added validation of user and expiration: https://docs.microsoft.com/en-us/aspnet/core/security/authentication/cookie#reacting-to-back-end-changes
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationScheme = "Cookies",
                LoginPath = new PathString("/Login"),
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
            });

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

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

            app.UseStaticFiles();

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
        }
    }
}
