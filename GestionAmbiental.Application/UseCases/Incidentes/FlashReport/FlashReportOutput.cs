using GestionAmbiental.Domain.Incidentes.Items;
using GestionAmbiental.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.FlashReport
{
    public class FlashReportOutput
    {
        public int Id_Flash_Report { get; set; }
        public string Codigo { get; set; }
        public int Id_Gerencia { get; set; }
        public string Gerencia_Nombre { get; set; }
        public int Id_Empresa { get; set; }
        public string Empresa_Nombre { get; set; }
        public string Lugar { get; set; }
        public DateTime Fecha_Hora { get; set; }
        public int Nivel_Incidente { get; set; }
		public string Nivel_Incidente_Descripcion { get; set; }
		public int Id_Componente_Ambiental { get; set; }
        public string Componente_Ambiental_Descripcion { get; set; }
        public int Id_Producto_Incidente { get; set; }
        public string Producto_Incidente_Nombre { get; set; }
        public float Cantidad_Derramada { get; set; }
        public int Id_Unidad_Cantidad_Derramada { get; set; }
        public string Unidad_Cantidad_Derramada_Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Registro_Fotografico { get; set; }
		public string Archivo_Registro_Fotografico { get; set; }
		public int Id_Supervisor_Marcobre { get; set; }
        public string Supervisor_Marcobre_Nombre { get; set; }
        public string Supervisor_Marcobre_Email { get; set; }
        public int? Id_Registrado_Por { get; set; }
        public string Registrado_Por_Nombre { get; set; }
        public string Supervisor_Contratista { get; set; }
        public string Cargo_Supervisor_Marcobre { get; set; }
        public string Cargo_Supervisor_Contratista { get; set; }
        public string Estado { get; set; }

        public FlashReportOutput(Domain.Incidentes.FlashReport.Flash_Report flashReport)
        {
            Id_Flash_Report = flashReport.Id_Flash_Report;
            Codigo = flashReport.Codigo;
            Id_Gerencia = flashReport.Id_Gerencia;
            Gerencia_Nombre = flashReport.Gerencia?.Descripcion ?? string.Empty;
            Id_Empresa = flashReport.Id_Empresa;
            Empresa_Nombre = flashReport.Empresa?.RazonSocial ?? string.Empty;
            Lugar = flashReport.Lugar;
            Fecha_Hora = flashReport.Fecha_Hora;
            Nivel_Incidente = flashReport.Id_Nivel_Incidente;
            Nivel_Incidente_Descripcion = flashReport.Nivel_Incidente.Descripcion;
			Id_Componente_Ambiental = flashReport.Id_Componente_Ambiental;
            Componente_Ambiental_Descripcion = flashReport.Componente_Ambiental.Descripcion;
            Id_Producto_Incidente = flashReport.Id_Producto_Incidente;
            Producto_Incidente_Nombre = flashReport.Producto_Incidente?.Descripcion ?? string.Empty;
            Cantidad_Derramada = flashReport.Cantidad_Derramada;
            Id_Unidad_Cantidad_Derramada = flashReport.Id_Unidad_Cantidad_Derramada;
            Unidad_Cantidad_Derramada_Nombre = flashReport.Unidad_Cantidad_Derramada?.Descripcion ?? string.Empty;
            Descripcion = flashReport.Descripcion;
            Registro_Fotografico = flashReport.Registro_Fotografico;
            Archivo_Registro_Fotografico = flashReport.Archivos_Registro_Fotografico;
            Id_Supervisor_Marcobre = flashReport.Id_Supervisor_Marcobre;
            Supervisor_Marcobre_Nombre = flashReport.Supervisor_Marcobre != null
                ? (flashReport.Supervisor_Marcobre.Nombres + " " + flashReport.Supervisor_Marcobre.Apellidos) : string.Empty;
            Supervisor_Marcobre_Email = flashReport.Supervisor_Marcobre != null
                ? flashReport.Supervisor_Marcobre.Email : string.Empty;
            Id_Registrado_Por = flashReport.Id_Registrado_Por;
            Registrado_Por_Nombre = flashReport.Registrado_Por != null
                ? (flashReport.Registrado_Por.Nombres + " " + flashReport.Registrado_Por.Apellidos) : string.Empty;
            Supervisor_Contratista = flashReport.Supervisor_Contratista;
            Cargo_Supervisor_Marcobre = flashReport.Cargo_Supervisor_Marcobre;
            Cargo_Supervisor_Contratista = flashReport.Cargo_Supervisor_Contratista;
            Estado = flashReport.Estado;
        }
    }
}
