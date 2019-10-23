using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess
{
    public class Context : DbContext
    {
        public Context(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Entities.Empresa_Contratista> Empresas_Contratistas { get; set; }
        public DbSet<Entities.Gerencia> Gerencias { get; set; }
        public DbSet<Entities.Inspeccion_Cabecera> Inspecciones_Cabecera { get; set; }
        public DbSet<Entities.Inspeccion_Detalle> Inspecciones_Detalle { get; set; }
        public DbSet<Entities.Log_Inspeccion> Log_Inspecciones { get; set; }
        public DbSet<Entities.Tipo_Condicion> Tipos_Condicion { get; set; }
        public DbSet<Entities.Usuario> Usuarios { get; set; }
        public DbSet<Entities.Rol> Rol { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Entities.Empresa_Contratista>()
               .ToTable("Tb_Empresa_Contratista");
            modelBuilder.Entity<Entities.Gerencia>()
                .ToTable("Tb_Gerencia");
            modelBuilder.Entity<Entities.Inspeccion_Cabecera>()
               .ToTable("Tb_Inspeccion_Cabecera");
            modelBuilder.Entity<Entities.Inspeccion_Detalle>()
               .ToTable("Tb_Inspeccion_Detalle");
            modelBuilder.Entity<Entities.Log_Inspeccion>()
               .ToTable("Tb_Log_Inspeccion");
            modelBuilder.Entity<Entities.Tipo_Condicion>()
                .ToTable("Tb_Tipo_Condicion");
            modelBuilder.Entity<Entities.Usuario>()
                .ToTable("Tb_Usuario");
            modelBuilder.Entity<Entities.Rol>()
                .ToTable("Tb_Rol");
        }
    }

}
