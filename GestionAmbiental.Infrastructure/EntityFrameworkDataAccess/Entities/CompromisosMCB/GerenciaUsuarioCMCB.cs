using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities.CompromisosMCB
{ 
    [Table("Tb_Gerencia_Usuario_CMCB")]
    public class GerenciaUsuarioCMCB
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "int")]
        public int? Id_Gerencia { get; set; }
        [Column(TypeName = "int")]
        public int? Id_Usuario { get; set; }
        [Column(TypeName ="varchar(255)")]
        public string? Estado { get; set; }
    }
}
