using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities.ResiduosSolidos
{
    [Table("Tb_Log_Ingreso_Salida_RS")]
    public class Log_Ingreso_Salida_RS
    {
        [Key]
        public int Id_Log_Ingreso_Salida_RS { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime Fecha { get; set; }
        [Column(TypeName = "varchar(6)")]
        public string Tipo { get; set; }
        [Column(TypeName = "int")]
        public int Id_Usuario { get; set; }
        [Column(TypeName = "text")]
        public string Bkp { get; set; }
        [Column(TypeName = "int")]
        public int Id_Ingreso_Salida_RS { get; set; }
    }
}
