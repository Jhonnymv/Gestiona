using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities.Test
{
    [Table("Tb_Test")]
    public class Test
    {
        [Key]
        public int Id { get; set; }


        [Column(TypeName = "varchar(50)")]
        public string Nombre { get; set; }


        [Column(TypeName = "int")]
        public int? Numero { get; set; }


        [Column(TypeName = "int")]
        public int? Id_Test_Lista { get; set; }


        [Column(TypeName = "decimal(10,2)")]
        public double? Decimal { get; set; }


        [Column(TypeName = "text")]
        public string Evidencia { get; set; }


        [Column(TypeName = "varchar(25)")]
        public string Estado { get; set; }
    }
}
