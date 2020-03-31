using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities.Incidentes.Items
{
    [Table("Tb_Nivel_Incidente")]
    public class Nivel_Incidente
    {
        [Key]
        public int Id_Nivel_Incidente { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string Descripcion { get; set; }
        [Column(TypeName = "varchar(3)")]
        public string Estado { get; set; }
    }
}
