using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities.CompromisosMCB
{
    [Table("Tb_Componentes_CMCB")]
    public class ComponenteCMCB
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "varchar(255)")]
        public string Tipo { get; set; }
        [Column(TypeName = "varchar(255)")]
        public string Componente { get; set; }
        [Column(TypeName = "varchar(255)")]
        public string? Servicio { get; set; }
    }
}
