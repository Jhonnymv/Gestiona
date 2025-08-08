using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace GestionAmbiental.UseCases.Incidentes.FlashReportMA
{
    public class EditarFlashReportModel
    {
		public string Id_Encriptado_Flash_Report { get; set; }
		public int Id_Flash_Report { get; set; }
		public string Codigo { get; set; }
		public int Id_Gerencia { get; set; }
		public int Id_Empresa { get; set; }
		public string Lugar { get; set; }
		public string Fecha_Incidente { get; set; }
		public string Hora_Incidente { get; set; }
		public DateTime Fecha_Hora { get; set; }
		public int Nivel_Incidente { get; set; }
		public int Id_Componente_Ambiental { get; set; }
        public string Componente_Ambiental { get; set; }
        public int Id_Producto_Incidente { get; set; }
		public float Cantidad_Derramada { get; set; }
		public int Id_Unidad_Cantidad_Derramada { get; set; }
		public string Descripcion { get; set; }
		public List<IFormFile> Evidencias { get; set; }
		public string Evidencias_Completas { get; set; }
		public string Evidencias_Nombres { get; set; }
		public int Id_Supervisor_Marcobre { get; set; }
		public string Supervisor_Contratista { get; set; }
		public string Cargo_Supervisor_Marcobre { get; set; }
		public string Cargo_Supervisor_Contratista { get; set; }
		public string Estado { get; set; }
		public string Opcion { get; set; }
		public string Acciones { get; set; }
		public int Id_Registrado_Por { get; set; }
    }
}
