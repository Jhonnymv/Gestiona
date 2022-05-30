using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities.MonitoreoCampoInterno.AvistamientoFlora
{
    [Table("Tb_MCIF_Fenologia_Avistamiento")]
    public class FenologiaAvistamiento
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "Int")]
        public int Id_Fenologia { get; set; }
        [Column(TypeName = "Int")]
        public int Id_Avistamiento { get; set; }
        [Column(TypeName = "Int")]
        public int Cantidad { get; set; }
        [Column(TypeName = "Text")]
        public string Descripcion { get; set; }
    }
}
