using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities.Incidentes.Items
{
    [Table("Tb_Tipo_Producto_Incidente")]
    public class Tipo_Producto_Incidente
    {
        [Key]
        public int Id_Tipo_Producto_Incidente { get; set; }
        [Column(TypeName = "varchar(10)")]
        public int Descripcion { get; set; }
        [Column(TypeName = "varchar(10)")]
        public int Estado { get; set; }
    }
}
