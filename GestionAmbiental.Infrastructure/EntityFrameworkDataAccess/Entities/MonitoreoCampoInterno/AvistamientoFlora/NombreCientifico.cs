using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities.MonitoreoCampoInterno.AvistamientoFlora
{
    [Table("Tb_MCIF_Nombre_Cientifico")]
    public class NombreCientifico
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "varhar(255)")]
        public string Nombre { get; set; }
        [Column(TypeName = "Text")]
        public string NombreComun { get; set; }
        [Column(TypeName = "Text")]
        public string Descripcion { get; set; }
    }
}
