using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities.ResiduosSolidos
{
    [Table("Tb_Conductor_RS")]
    public class Conductor_RS
    {
        [Key]
        public int Id_Conductor_RS { get; set; }
        [Column(TypeName ="varchar(100)")]
        public string Nombres { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Apellidos { get; set; }
        [Column(TypeName = "varchar(10)")]
        public string NLicencia { get; set; }
        [Column(TypeName = "varchar(3)")]
        public string Estado { get; set; }
    }
}
