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
            //if (env.EnvironmentName == "Development")
            if (env.EnvironmentName == "Production")
            {
                //app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
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
