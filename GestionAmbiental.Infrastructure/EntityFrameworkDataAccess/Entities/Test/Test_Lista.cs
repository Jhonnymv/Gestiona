using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities.Test
{
    [Table("Tb_Test_Lista")]
    public class Test_Lista
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string Nombre { get; set; }

        [Column(TypeName = "varchar(3)")]
        public string Estado { get; set; }

    }
}
