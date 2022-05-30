using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities.MonitoreoCampoInterno.AvistamientoFlora
{
    [Table("Tb_MCIF_Zonas")]
    public class Zona
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "varhar(255)")]
        public string Nombre { get; set; }
        [Column(TypeName = "Text")]
        public string Descripcion { get; set; }
    }
}
