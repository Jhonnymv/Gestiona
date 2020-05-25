using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities.Incidentes.InvestigacionFinal
{
    [Table("Tb_Factor_Causa_Incidente")]
    public class Factor_Causa_Incidente
    {
        [Key]
        public int Id_Factor_Causa_Incidente { get; set; }
        [Column(TypeName = "varhcar(250)")]
        public string Descripcion { get; set; }
        [Column(TypeName = "varhcar(20)")]
        public string Tipo_Factor { get; set; }
        [Column(TypeName = "varhcar(3)")]
        public string Estado { get; set; }
    }
}
