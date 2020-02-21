using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities.ResiduosSolidos
{
    [Table("Tb_Ingreso_Salida")]
    public class Ingreso_Salida
    {
        [Key]
        public int Id_Ingreso_Salida { get; set; }
        [Column(TypeName = "varchar(10)")]
        public string Descripcion { get; set; }
        [Column(TypeName = "varchar(3)")]
        public string Estado { get; set; }
    }
}
