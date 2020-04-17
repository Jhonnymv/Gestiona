using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities.Incidentes.InvestigacionFinal
{
    [Table("Tb_Causa_Incidente")]
    public class Causa_Incidente
    {
        [Key]
        public int Id_Causa_Incidente { get; set; }
        [Column(TypeName = "int")]
        public int Id_Reporte_Preliminar { get; set; }
        [Column(TypeName = "int")]
        public int? Id_Investigacion_Final { get; set; }
        [Column(TypeName = "int")]
        public int? Id_Acto_Condicion { get; set; }
        [Column(TypeName = "varhcar(25)")]
        public string Tipo_Acto_Condicion { get; set; }
        [Column(TypeName = "int")]
        public int? Id_Factor_Causa_Incidente { get; set; }
        [Column(TypeName = "varhcar(100)")]
        public string Descripcion { get; set; }
        [Column(TypeName = "varhcar(15)")]
        public string Estado { get; set; }
    }
}
