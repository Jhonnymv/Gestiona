using Autofac;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess
{
    public class Module : Autofac.Module
    {
        public string ConnectionString { get; set; }

        protected override void Load(ContainerBuilder builder)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DbContext>();
            optionsBuilder.UseSqlServer(ConnectionString);
            optionsBuilder.EnableSensitiveDataLogging(true);

            builder.RegisterType<Context>()
              .WithParameter(new TypedParameter(typeof(DbContextOptions), optionsBuilder.Options))
              .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(InfrastructureException).Assembly)
                .Where(type => type.Namespace.Contains("EntityFrameworkDataAccess"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}
