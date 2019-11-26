using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;



namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities
{
    [Table("Tb_Gerencia")]
    public class Gerencia
    {
        [Key]
        public int Id_Gerencia { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string Descripcion { get; set; }
    }
}
