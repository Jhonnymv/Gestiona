using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities.ResiduosSolidos
{
    [Table("Tb_Residuo_Solido")]
    public class Residuo_Solido
    {
        [Key]
        public int Id_Residuo_Solido { get; set; }
        [Column(TypeName = "int")]
        public int Id_Tipo_Residuo { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string Descripcion { get; set; }
        [Column(TypeName = "varchar(3)")]
        public string Estado { get; set; }
    }
}
