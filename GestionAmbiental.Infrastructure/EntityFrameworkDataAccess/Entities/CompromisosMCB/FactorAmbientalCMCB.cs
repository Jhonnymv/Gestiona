using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities.CompromisosMCB
{
    [Table("Tb_CMCB_FactorAmbiental")]
    public class FactorAmbientalCMCB
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "varchar(255)")]
        public string Factor { get; set; }
        [Column(TypeName = "text")]
        public string Descripcion { get; set; }
    }
   
}
