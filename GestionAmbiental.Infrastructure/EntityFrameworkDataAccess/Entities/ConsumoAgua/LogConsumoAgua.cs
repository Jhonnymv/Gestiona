using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities.ConsumoAgua
{
    [Table("Tb_Log_Consumo_Agua")]
    public class LogConsumoAgua
    {
        [Key]
        public int Id_Log_Consumo_Agua { get; set; }
        public DateTime Fecha { get; set; }
        [Column(TypeName = "varchar(6)")]
        public string Tipo { get; set; }
        [Column(TypeName = "int")]
        public int Id_Usuario { get; set; }
        [Column(TypeName = "text")]
        public string Bkp { get; set; }
        [Column(TypeName = "int")]
        public int Id_Consumo_Agua { get; set; }
    }
}
