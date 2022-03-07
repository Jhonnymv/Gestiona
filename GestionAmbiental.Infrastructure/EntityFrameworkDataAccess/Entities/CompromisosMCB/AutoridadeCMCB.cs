using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities.CompromisosMCB
{
    [Table("Tb_Autoridades_CMCB")]
    public class AutoridadeCMCB
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName ="varchar(255)")]
        public string Autoridad { get; set; }
    }
}
