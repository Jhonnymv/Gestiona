using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities.CompromisosMCB
{
    [Table("Tb_Medios_CMCB")]
    public class MedioVerificacionCMCB
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName ="varchar(255)")]
        public string Medio { get; set; }
    }
}
