using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities.ResiduosSolidos
{
    [Table("Tb_Tipo_Residuo")]
    public class Tipo_Residuo
    {
        [Key]
        public int Id_Tipo_Residuo { get; set; }
        [Column(TypeName = "varchar(30)")]
        public string Descripcion { get; set; }
        [Column(TypeName = "varchar(3)")]
        public string Estado { get; set; }
    }
}
