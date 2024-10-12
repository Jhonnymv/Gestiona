using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities.Inspecciones.Items
{
    [Table("Tb_Tipo_Inspeccion")]
    public class Tipo_Inspeccion
    {
        [Key]
        public int Id_Tipo_Inspeccion { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string Descripcion { get; set; }
    }
}
