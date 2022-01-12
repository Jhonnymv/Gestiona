using Autofac;
using Autofac.Configuration;
using GestionAmbiental.UseCases;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace GestionAmbiental
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(60);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                //options.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.Always;
                options.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.Always;
                options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict;
            });

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.Always;
                options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict;
            });

            services.AddControllersWithViews()         
                .AddFeatureFolders(new OdeToCode.AddFeatureFolders.FeatureFolderOptions()
                { FeatureFolderName = "UseCases" });

            services.AddSingleton<DataProtectionPurpouseStrings>();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new ConfigurationModule(Configuration));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.EnvironmentName == "Development")
            //if (env.EnvironmentName == "Production")
            {
                //app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }

            app.UseCookiePolicy(
            new CookiePolicyOptions
            {
                HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always,
                Secure = Microsoft.AspNetCore.Http.CookieSecurePolicy.Always,
                MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.Lax
                
            });

            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
                //context.Response.Headers.Add("Content-Security-Policy", "default-src 'self'; https://cdnjs.cloudflare.com; https://fonts.googleapis.com; http://www.w3.org;");
                context.Response.Headers.Add("Content-Security-Policy", "frame-ancestors 'self';"
                    //"default-src 'self';" +
                    //"img-src 'self' *; " +
                    //"script-src 'self' 'unsafe-hashes' 'sha256-b7Qvpm/r4bsZujSrFc+ogWv0a63hGRYnOEXWfrZXH9Y=' 'sha256-g7ij96E5PsbD/0KPQ/l9J8MxLYPKdDuyQAJ0Fd3UaC0=' cdnjs.cloudflare.com cdn.jsdelivr.net;" +
                    //"style-src 'self'  fonts.googleapis.com;" +
                    //"font-src 'self' fonts.gstatic.com;" 
                    //"unsafe-hashes 'sha256-b7Qvpm/r4bsZujSrFc+ogWv0a63hGRYnOEXWfrZXH9Y=' 'sha256-g7ij96E5PsbD/0KPQ/l9J8MxLYPKdDuyQAJ0Fd3UaC0=' 'sha256-biLFinpqYMtWHmXfkA1BPeCY0/fNt46SAZ+BBk5YUog=' 'sha256-eSZwEJ3H1i8yg7R0pu22KupiMs/BIhDtVv9psceOrBE=';"
                    );
                //context.Response.Headers.Add("Content-Security-Policy", "style-src-attr 'sha256-b7Qvpm/r4bsZujSrFc+ogWv0a63hGRYnOEXWfrZXH9Y=' 'unsafe-hashes';");
                await next();
            });

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseSession();

            app.UseHttpContextItemsMiddleware(); 

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}");
            });
        }
    }
}
