using GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities.Inspecciones;
using GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities.RAC;
using GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities.ResiduosSolidos;
using GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities;
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
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<Gerencia> Gerencias { get; set; }
        public DbSet<Inspeccion_Cabecera> Inspecciones_Cabecera { get; set; }
        public DbSet<Inspeccion_Detalle> Inspecciones_Detalle { get; set; }
        public DbSet<Log_Inspeccion> Log_Inspecciones { get; set; }
        public DbSet<Tipo_Condicion> Tipos_Condicion { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Rol { get; set; }
        public DbSet<Info_SGA> Info_SGAs { get; set; }
        public DbSet<RAC> RACs { get; set; }
        public DbSet<Log_RAC> Log_RACs { get; set; }
        public DbSet<ACR_CMRS> ACR_CMRs { get; set; }
        public DbSet<EO_RS> EO_RSs { get; set; }
        public DbSet<Ingreso_Salida> Ingresos_Salidas { get; set; }
        public DbSet<Manifiesto_RS> Manifiesto_RSs { get; set; }
        public DbSet<Motivo_Ingreso_Salida> Motivos_Ingreso_Salida { get; set; }
        public DbSet<Ingreso_Salida_RS> Ingresos_Salidas_RS { get; set; }
        public DbSet<Residuo_Solido> Residuos_Solidos { get; set; }
        public DbSet<Tipo_Residuo> Tipo_Residuos { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Empresa>()
               .ToTable("Tb_Empresa");
            modelBuilder.Entity<Gerencia>()
                .ToTable("Tb_Gerencia");
            modelBuilder.Entity<Inspeccion_Cabecera>()
               .ToTable("Tb_Inspeccion_Cabecera");
            modelBuilder.Entity<Inspeccion_Detalle>()
               .ToTable("Tb_Inspeccion_Detalle");
            modelBuilder.Entity<Log_Inspeccion>()
               .ToTable("Tb_Log_Inspeccion");
            modelBuilder.Entity<Tipo_Condicion>()
                .ToTable("Tb_Tipo_Condicion");
            modelBuilder.Entity<Usuario>()
                .ToTable("Tb_Usuario");
            modelBuilder.Entity<Rol>()
                .ToTable("Tb_Rol");
            modelBuilder.Entity<Info_SGA>()
                .ToTable("Tb_Info_SGA");
            modelBuilder.Entity<RAC>()
                .ToTable("Tb_RAC");
            modelBuilder.Entity<Log_RAC>()
                .ToTable("Tb_Log_RAC");
            modelBuilder.Entity<ACR_CMRS>()
                .ToTable("Tb_ACR_CMRS");
            modelBuilder.Entity<EO_RS>()
                .ToTable("Tb_EO_RS");
            modelBuilder.Entity<Ingreso_Salida>()
                .ToTable("Tb_Ingreso_Salida");
            modelBuilder.Entity<Manifiesto_RS>()
                .ToTable("Tb_Manifiesto_RS");
            modelBuilder.Entity<Motivo_Ingreso_Salida>()
                .ToTable("Tb_Motivo_Ingreso_Salida");
            modelBuilder.Entity<Ingreso_Salida_RS>()
                .ToTable("Tb_Ingreso_Salida_RS");
            modelBuilder.Entity<Residuo_Solido>()
                .ToTable("Tb_Residuo_Solido");
            modelBuilder.Entity<Tipo_Residuo>()
                .ToTable("Tb_Tipo_Residuo");
        }
    }

}
