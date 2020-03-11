using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities.ResiduosSolidos
{
    [Table("Tb_Responsable_ACR_CRMS")]
    public class Responsable_ACR_CRMS
    {
        [Key]
        public int Id_Responsable_ARC_CRMS { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Nombres { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Apellidos { get; set; }
        [Column(TypeName = "varchar(3)")]
        public string Estado { get; set; }
    }
}
