using Autofac;
using Autofac.Configuration;
using GestionAmbiental.UseCases;
using GestionAmbiental.UseCases.Util.Implementation;
using GestionAmbiental.UseCases.Util.Interfaces;
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
                options.IdleTimeout = TimeSpan.FromMinutes(15);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
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
            services.AddScoped<IGestorArchivos, GestorArchivos>();
            services.AddScoped<IGestorEncriptado, GestorEncriptado>();
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
                context.Response.Headers.Add("Strict-Transport-Security", "max-age=31536000 ; includeSubDomains");
                context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
                context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                //context.Response.Headers.Add("Content-Security-Policy", "default-src 'self'; object-src 'none'; frame-ancestors 'none'; upgrade-insecure-requests; block-all-mixed-content");
                context.Response.Headers.Add("Content-Security-Policy", "frame-ancestors 'self';");
                context.Response.Headers.Add("X-Permitted-Cross-Domain-Policies", "none");
                context.Response.Headers.Add("Referrer-Policy", "no-referrer");
                //context.Response.Headers.Add("Clear-Site-Data", "\"cache\",\"cookies\",\"storage\"");
                context.Response.Headers.Add("Clear-Site-Data", "\"storage\"");
                context.Response.Headers.Add("Cross-Origin-Embedder-Policy", "require-corp");
                context.Response.Headers.Add("Cross-Origin-Opener-Policy", "same-origin");
                context.Response.Headers.Add("Cross-Origin-Resource-Policy", "same-origin");
                context.Response.Headers.Add("Cache-Control", "no-store, max-age=0");
                context.Response.Headers.Add("Pragma", "no-cache");
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