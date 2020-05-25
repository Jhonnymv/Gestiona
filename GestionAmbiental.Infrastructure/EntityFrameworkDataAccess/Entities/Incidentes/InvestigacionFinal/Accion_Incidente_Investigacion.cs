using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities.Incidentes.InvestigacionFinal
{
    [Table("Tb_Accion_Incidente_Investigacion")]
    public class Accion_Incidente_Investigacion
    {
        [Key]
        public int Id_Accion_Incidente_Investigacion { get; set; }
        [Column(TypeName = "int")]
        public int? Id_Investigacion_Final { get; set; }
        [Column(TypeName = "varchar(15)")]
        public string Tipo_Accion { get; set; }
        [Column(TypeName = "varchar(250)")]
        public string Accion_Inmediata { get; set; }
        [Column(TypeName = "int")]
        public int? Id_Usuario { get; set; }
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
