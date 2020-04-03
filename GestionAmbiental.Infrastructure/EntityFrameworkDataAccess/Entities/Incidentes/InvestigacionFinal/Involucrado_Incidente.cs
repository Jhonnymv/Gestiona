using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities.Incidentes.InvestigacionFinal
{
    [Table("Tb_Involucrado_Incidente")]
    public class Involucrado_Incidente
    {
        [Key]
        public int Id_Involucrado_Incidente { get; set; }
        [Column(TypeName = "int")]
        public int Id_Reporte_Preliminar { get; set; }
        [Column(TypeName = "int")]
        public int Id_Investigacion_Final { get; set; }
        [Column(TypeName = "int")]
        public int? Id_Usuario { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Nombres { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Apellidos { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Doc_Identidad { get; set; }
        [Column(TypeName = "int")]
        public int? Edad { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Puesto_Trabajo { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Empresa { get; set; }
        [Column(TypeName = "varchar(10)")]
        public string Turno { get; set; }
        [Column(TypeName = "varchar(250)")]
        public string Descripcion { get; set; }
        [Column(TypeName = "varchar(10)")]
        public string Tipo_Involucrado { get; set; }
        [Column(TypeName = "varchar(3)")]
        public string Estado { get; set; }
    }
}
