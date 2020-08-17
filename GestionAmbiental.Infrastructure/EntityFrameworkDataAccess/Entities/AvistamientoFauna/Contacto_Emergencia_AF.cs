using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities.AvistamientoFauna
{
    [Table("Tb_Contacto_Emergencia_AF")]
    public class Contacto_Emergencia_AF
    {
        [Key]
        public int Id_Contacto_Emergencia { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Nombres { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Apellidos { get; set; }
        [Column(TypeName = "varchar(15)")]
        public string Telefono { get; set; }
        [Column(TypeName = "varchar(3)")]
        public string Estado { get; set; }
    }
}
