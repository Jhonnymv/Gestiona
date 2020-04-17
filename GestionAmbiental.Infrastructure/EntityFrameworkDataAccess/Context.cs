using GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities;
using GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities.Incidentes.InvestigacionFinal;
using GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities.Incidentes.Items;
using GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities.Incidentes.ReportePreliminar;
using GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities.Inspecciones;
using GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities.RAC;
using GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities.ResiduosSolidos;
using Microsoft.EntityFrameworkCore;

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
        public DbSet<Responsable_ACR_CRMS> Responsables_ACR_CRM { get; set; }
        public DbSet<Ingreso_Salida_RS> Ingresos_Salidas_RS { get; set; }
        public DbSet<Residuo_Solido> Residuos_Solidos { get; set; }
        public DbSet<Tipo_Residuo> Tipo_Residuos { get; set; }
        public DbSet<Conductor_RS> Condutores_RS { get; set; }
        public DbSet<Vehiculo_RS> Vehiculos_RS { get; set; }
        public DbSet<Log_Ingreso_Salida_RS> Log_Ingreso_Salida_RSs { get; set; }
        public DbSet<Log_Manifiesto_RS>Log_Manifiesto_RS { get; set; }
        public DbSet<Usuario_Notificar_RS> Usuarios_Notificar_RS { get; set; }

        public DbSet<Componente_Ambiental> Componente_Ambiental { get; set; }
        public DbSet<Especie_Fauna> Especie_Fauna { get; set; }
        public DbSet<Especie_Flora> Especie_Flora { get; set; }
        public DbSet<Estado_Equipo_Incidente> Estado_Equipo_Incidente { get; set; }
        public DbSet<Lugar_Disposicion> Lugar_Disposicion { get; set; }
        public DbSet<Nivel_Incidente> Nivel_Incidente { get; set; }
        public DbSet<Producto_Incidente> Producto_Incidente { get; set; }
        public DbSet<Tipo_Agua> Tipo_Agua { get; set; }
        public DbSet<Tipo_Incidente> Tipo_Incidente { get; set; }
        public DbSet<Tipo_Producto_Incidente> Tipo_Producto_Incidente { get; set; }
        public DbSet<Tipo_Suelo> Tipo_Suelo { get; set; }
        public DbSet<Unidad_Medida> Unidad_Medida { get; set; }

        public DbSet<Reporte_Preliminar_Incidente> Reporte_Preliminar_Incidente { get; set; }

        public DbSet<Accion_Correctiva_Incidente> Accion_Correctiva_Incidente { get; set; }
        public DbSet<Factor_Causa_Incidente> Factor_Causa_Incidente { get; set; }
        public DbSet<Causa_Incidente> Causa_Incidente { get; set; }
        public DbSet<Impacto_Ambiental_Incidente> Impacto_Ambiental_Incidente { get; set; }
        public DbSet<Investigacion_Final_Incidente> Investigacion_Final_Incidente { get; set; }
        public DbSet<Involucrado_Incidente> Involucrado_Incidente { get; set; }
        public DbSet<Producto_Derramado_Incidente> Producto_Derramado_Incidente { get; set; }

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
            modelBuilder.Entity<Responsable_ACR_CRMS>()
                .ToTable("Tb_Responsable_ACR_CRMS");
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
            modelBuilder.Entity<Conductor_RS>()
                .ToTable("Tb_Conductor_RS");
            modelBuilder.Entity<Vehiculo_RS>()
                .ToTable("Tb_Vehiculo_RS");
            modelBuilder.Entity<Log_Ingreso_Salida_RS>()
                .ToTable("Tb_Log_Ingreso_Salida_RS");
            modelBuilder.Entity<Log_Manifiesto_RS>()
                .ToTable("Tb_Log_Manifiesto_RS");
            modelBuilder.Entity<Usuario_Notificar_RS>()
                .ToTable("Tb_Usuarios_Notificar_RS");

            modelBuilder.Entity<Componente_Ambiental>()
                .ToTable("Tb_Componente_Ambiental");
            modelBuilder.Entity<Especie_Fauna>()
                .ToTable("Tb_Especie_Fauna");
            modelBuilder.Entity<Especie_Flora>()
                .ToTable("Tb_Especie_Flora");
            modelBuilder.Entity<Estado_Equipo_Incidente>()
                .ToTable("Tb_Estado_Equipo_Incidente");
            modelBuilder.Entity<Lugar_Disposicion>()
                .ToTable("Tb_Lugar_Disposicion");
            modelBuilder.Entity<Nivel_Incidente>()
                .ToTable("Tb_Nivel_Incidente");
            modelBuilder.Entity<Producto_Incidente>()
                .ToTable("Tb_Producto_Incidente");
            modelBuilder.Entity<Tipo_Agua>()
                .ToTable("Tb_Tipo_Agua");
            modelBuilder.Entity<Tipo_Incidente>()
                .ToTable("Tb_Tipo_Incidente");
            modelBuilder.Entity<Tipo_Producto_Incidente>()
                .ToTable("Tb_Tipo_Producto_Incidente");
            modelBuilder.Entity<Tipo_Suelo>()
                .ToTable("Tb_Tipo_Suelo");
            modelBuilder.Entity<Unidad_Medida>()
                .ToTable("Tb_Unidad_Medida");

            modelBuilder.Entity<Reporte_Preliminar_Incidente>()
                .ToTable("Tb_Reporte_Preliminar_Incidente");

            modelBuilder.Entity<Accion_Correctiva_Incidente>()
                .ToTable("Tb_Accion_Correctiva_Incidente");
            modelBuilder.Entity<Factor_Causa_Incidente>()
                .ToTable("Tb_Factor_Causa_Incidente");
            modelBuilder.Entity<Causa_Incidente>()
                .ToTable("Tb_Causa_Incidente");
            modelBuilder.Entity<Impacto_Ambiental_Incidente>()
                .ToTable("Tb_Impacto_Ambiental_Incidente");
            modelBuilder.Entity<Investigacion_Final_Incidente>()
                .ToTable("Tb_Investigacion_Final_Incidente");
            modelBuilder.Entity<Involucrado_Incidente>()
                .ToTable("Tb_Involucrado_Incidente");
            modelBuilder.Entity<Producto_Derramado_Incidente>()
                .ToTable("Tb_Producto_Derramado_Incidente");
        }
    }
}
