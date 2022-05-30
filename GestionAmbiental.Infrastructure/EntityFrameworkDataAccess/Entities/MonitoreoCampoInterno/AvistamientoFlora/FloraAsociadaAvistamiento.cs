using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities.MonitoreoCampoInterno.AvistamientoFlora
{
    [Table("Tb_MCIF_FloraAsociada_Avistamiento") ]
    public class FloraAsociadaAvistamiento
    {
        public int Id { get; set; }
        [Column(TypeName = "Int")]
        public int Id_Avistamiento { get; set; }
        [Column(TypeName = "Int")]
        public int Id_FloraAsociada { get; set; }
        [Column(TypeName = "Int")]
        public int Cantidad { get; set; }
        [Column(TypeName = "Text")]
        public string Descripcion { get; set; }
    }
}
