using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities
{
    [Table("Tb_Area_Trabajo")]
    public class Area_Trabajo
    {
        [Key]
        public int Id_Area_Trabajo { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Descripcion { get; set; }
        [Column(TypeName = "varchar(3)")]
        public string Estado { get; set; }
    }
}
