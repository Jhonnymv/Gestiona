using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities.CompromisosMCB
{
    [Table("Tb_Gerencia_CMCB")]
    public class GerenciaCMCB
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "varchar(255)")]
        public string Gerencia { get; set; }

        [Column(TypeName = "varchar(255)")]
        public string Descripcion { get; set; }
    }
}
