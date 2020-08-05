using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;


namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities
{
    [Table("Tb_Empresa")]
    public class Empresa
    {
        [Key]
        public int Id_Empresa { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Razon_Social { get; set; }
        [Column(TypeName = "varchar(3)")]
        public string Estado { get; set; }

    }
}
