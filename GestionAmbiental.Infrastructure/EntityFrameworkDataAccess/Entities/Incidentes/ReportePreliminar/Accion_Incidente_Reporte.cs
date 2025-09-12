using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities.Incidentes.ReportePreliminar
{
    [Table("Tb_Accion_Incidente_Reporte")]
    public class Accion_Incidente_Reporte
    {
        [Key]
        public int Id_Accion_Incidente_Reporte { get; set; }
        [Column(TypeName = "int")]
        public int Id_Reporte_Preliminar { get; set; }
        [Column(TypeName = "varchar(15)")]
        public string Tipo_Accion { get; set; }
        [Column(TypeName = "varchar(1100)")]
        public string Accion_Inmediata { get; set; }
        [Column(TypeName = "int")]
        public int? Id_Usuario { get; set; }
        [Column(TypeName = "int")]
        public int? Id_Usuario2 { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Responsable { get; set; }
        [Column(TypeName = "text")]
        public string Evidencia { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Fecha { get; set; }
        [Column(TypeName = "varchar(10)")]
        public string Estatus { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Fecha_Implementacion { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Comentarios_Implementacion { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Fecha_Verificacion { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Comentarios_Verificacion { get; set; }
        [Column(TypeName = "varchar(15)")]
        public string Estado { get; set; }
    }
}
