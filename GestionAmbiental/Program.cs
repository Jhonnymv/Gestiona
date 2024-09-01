using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Autofac.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using Microsoft.Extensions.Hosting;
using System;

namespace GestionAmbiental
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new HostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureServices(services => services.AddAutofac())
                    .UseStartup<Startup>();
                })
                .UseSerilog((hostingContext, loggerConfiguration) =>
                {
                    loggerConfiguration.MinimumLevel.Debug()
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                    .Enrich.FromLogContext()
                    .WriteTo.File(Path.Combine(hostingContext.HostingEnvironment.ContentRootPath, "logs/log-" + DateTime.Today.ToString("yyyyMMdd") + ".log"));
                })
                .ConfigureAppConfiguration((context,config)=>
                {
                    var env = context.HostingEnvironment;
                    config.AddJsonFile("autofac.json");
                    config.AddEnvironmentVariables();
                })
                .Build();

            host.Run();
        }        
    }
}
