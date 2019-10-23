using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;


namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities
{
    [Table("Tb_Rol")]
    public class Rol
    {
        [Key]
        public int Id_Rol { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string Descripcion { get; set; }
    }
}
