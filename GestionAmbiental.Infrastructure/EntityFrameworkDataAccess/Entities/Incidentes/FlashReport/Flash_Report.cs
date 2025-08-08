using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities.Incidentes.FlashReport
{
    [Table("Tb_Flash_Report")]
    public class Flash_Report
    {
        [Key]
        public int Id_Flash_Report { get; set; }
        [Column(TypeName = "varchar(10)")]
        public string Codigo { get; set; }
        [Column(TypeName = "int")]
        public int Id_Gerencia { get; set; }
        [Column(TypeName = "int")]
        public int Id_Empresa { get; set; }
        [Column(TypeName = "varchar(500)")]
        public string Lugar { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime Fecha_Hora { get; set; }
        [Column(TypeName = "int")]
        public int Nivel_Incidente { get; set; }
        [Column(TypeName = "int")]
        public int Id_Componente_Ambiental { get; set; }
        [Column(TypeName = "int")]
        public int Id_Producto_Incidente { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public float Cantidad_Derramada { get; set; }
        [Column(TypeName = "int")]
        public int Id_Unidad_Cantidad_Derramada { get; set; }
        [Column(TypeName = "text")]
        public string Descripcion { get; set; }
		[Column(TypeName = "text")]
		public string Archivos_Registro_Fotografico { get; set; }
		[Column(TypeName = "text")]
        public string Registro_Fotografico { get; set; }
        [Column(TypeName = "int")]
        public int Id_Supervisor_Marcobre { get; set; }
        [Column(TypeName = "varchar(1000)")]
        public string Supervisor_Contratista { get; set; }
        [Column(TypeName = "varchar(1000)")]
        public string Cargo_Supervisor_Marcobre { get; set; }
        [Column(TypeName = "varchar(1000)")]
        public string Cargo_Supervisor_Contratista { get; set; }
        [Column(TypeName = "varchar(15)")]
        public string Estado { get; set; }
        [Column(TypeName = "int")]
        public int? Id_Registrado_Por { get; set; }
    }
}
