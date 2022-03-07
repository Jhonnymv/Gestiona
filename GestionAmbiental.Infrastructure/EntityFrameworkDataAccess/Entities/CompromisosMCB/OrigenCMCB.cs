using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities.CompromisosMCB
{
    [Table("AreaCMCB")]
    public class OrigenesCMCM
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "varchar(255)")]
        public string Origen { get; set; }
    }
}
