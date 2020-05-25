using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities.Incidentes.InvestigacionFinal
{
    [Table("Tb_Producto_Derramado_Incidente")]
    public class Producto_Derramado_Incidente
    {
        [Key]
        public int Id_Producto_Derramado_Incidente { get; set; }
        [Column(TypeName = "int")]
        public int Id_Investigacion_Final { get; set; }
        [Column(TypeName = "int")]
        public int? Id_Producto_Incidente { get; set; }
        [Column(TypeName = "int")]
        public int? Id_Tipo_Producto { get; set; }
        [Column(TypeName = "varchar(10)")]
        public string Estado_Producto { get; set; }
        [Column(TypeName = "decimal(6,2)")]
        public double? Cantidad_Derramada { get; set; }
        [Column(TypeName = "int")]
        public int? Id_Unidad { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Equipo_Responsable { get; set; }
        [Column(TypeName = "int")]
        public int? Id_Estado_Equipo_Responsable { get; set; }
        [Column(TypeName = "decimal(6,2)")]
        public double? Cantidad_Recuperado { get; set; }
        [Column(TypeName = "decimal(6,2)")]
        public double? Cantidad_No_Recuperado { get; set; }
        [Column(TypeName = "varchar(15)")]
        public string Estado { get; set; }
    }
}
