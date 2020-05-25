using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities.Incidentes.ReportePreliminar
{
    [Table("Tb_Involucrado_Incidente_Reporte")]
    public class Involucrado_Incidente_Reporte
    {
        [Key]
        public int Id_Involucrado_Incidente_Reporte { get; set; }
        [Column(TypeName = "int")]
        public int? Id_Reporte_Preliminar { get; set; }
        [Column(TypeName = "int")]
        public int? Id_Usuario { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Nombres { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Apellidos { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Email { get; set; }
        [Column(TypeName = "varchar(12)")]
        public string Celular { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Puesto_Trabajo { get; set; }
        [Column(TypeName = "varchar(10)")]
        public string Turno { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string Tipo_Involucrado { get; set; }
        [Column(TypeName = "varchar(15)")]
        public string Estado { get; set; }
    }
}
