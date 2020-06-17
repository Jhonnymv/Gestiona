using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities.Incidentes.Items
{
    [Table("Tb_Unidad_Medida")]
    public class Unidad_Medida
    {
        [Key]
        public int Id_Unidad_Medida { get; set; }
        [Column(TypeName = "varchar(25)")]
        public string Descripcion { get; set; }
        [Column(TypeName = "varchar(10)")]
        public string Tipo { get; set; }
        [Column(TypeName = "varchar(3)")]
        public string Estado { get; set; }
    }
}
