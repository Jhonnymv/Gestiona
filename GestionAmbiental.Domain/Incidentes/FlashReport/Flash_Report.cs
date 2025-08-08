using GestionAmbiental.Domain.Incidentes.Items;
using GestionAmbiental.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Domain.Incidentes.FlashReport
{
    public class Flash_Report
    {
        public int Id_Flash_Report { get; set; }
        public string Codigo { get; set; }
        public int Id_Gerencia { get; set; }
        public Gerencia? Gerencia { get; set; }
        public int Id_Empresa { get; set; }
        public Empresa? Empresa { get; set; }
        public string Lugar { get; set; }
        public DateTime Fecha_Hora { get; set; }
        public int Id_Nivel_Incidente { get; set; }
        public Nivel_Incidente? Nivel_Incidente { get; set; }
        public int Id_Componente_Ambiental { get; set; }
        public Componente_Ambiental Componente_Ambiental { get; set; }
        public int Id_Producto_Incidente { get; set; }
        public Producto_Incidente? Producto_Incidente { get; set; }
        public float Cantidad_Derramada { get; set; }
        public int Id_Unidad_Cantidad_Derramada { get; set; }
        public Unidad_Medida? Unidad_Cantidad_Derramada { get; set; }
        public string Descripcion { get; set; }
        public string Registro_Fotografico { get; set; }
		public string Archivos_Registro_Fotografico { get; set; }
		public int Id_Supervisor_Marcobre { get; set; }
        public Usuario? Supervisor_Marcobre { get; set; }
        public string Supervisor_Contratista { get; set; }
        public string Cargo_Supervisor_Marcobre { get; set; }
        public string Cargo_Supervisor_Contratista { get; set; }
        public string Estado { get; set; }
        public int? Id_Registrado_Por { get; set; }
        public Usuario? Registrado_Por { get; set; }
    }
}
