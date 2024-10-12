using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities.Inspecciones.Items
{
    [Table("Tb_Miembro_Comite_Inspeccion")]
    public class Miembro_Comite_Inspeccion
    {
        [Key]
        public int Id_Miembro_Comite { get; set; }

        [Column(TypeName = "varchar(MAX)")]
        public string Nombres { get; set; }

        [Column(TypeName = "varchar(MAX)")]
        public string Apellidos { get; set; }

        [Column(TypeName = "varchar(3)")]
        public string Estado { get; set; }
    }
}
