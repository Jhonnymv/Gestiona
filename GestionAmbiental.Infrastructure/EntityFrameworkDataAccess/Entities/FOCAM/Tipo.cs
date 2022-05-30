using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities.FOCAM
{
    [Table("Tb_FOCAM_Tipo")]
    public class Tipo
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "varhar(255)")]
        public string Nombre { get; set; }
        [Column(TypeName = "Text")]
        public string Descripcion { get; set; }
    }
}
