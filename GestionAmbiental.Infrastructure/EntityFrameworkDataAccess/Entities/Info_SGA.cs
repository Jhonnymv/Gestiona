using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities
{
    [Table("Tb_Info_SGA")]
    public class Info_SGA
    {
        [Key]
        public int Id_Info { get; set; }
        [Column(TypeName = "varchar(300)")]
        public string Info { get; set; }
        [Column(TypeName = "varchar(30)")]
        public string Tipo { get; set; }
    }
}
