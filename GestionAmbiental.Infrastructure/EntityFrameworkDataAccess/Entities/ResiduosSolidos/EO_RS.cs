using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities.ResiduosSolidos
{
    [Table("Tb_EO_RS")]
    public class EO_RS
    {
        [Key]
        public int Id_EO_RS { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Razon_Social { get; set; }
        [Column(TypeName = "varchar(10)")]
        public string Registro { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime Fecha_Vencimiento_Registro { get; set; }
        [Column(TypeName = "varchar(3)")]
        public string Estado { get; set; }
    }
}
