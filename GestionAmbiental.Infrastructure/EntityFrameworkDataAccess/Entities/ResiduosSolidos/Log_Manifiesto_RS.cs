using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities.ResiduosSolidos
{
    [Table("Tb_Log_Manifiesto_RS")]
    public class Log_Manifiesto_RS
    {
        [Key]
        public int Id_Log_Manifiesto_RS { get; set; }
        public DateTime Fecha { get; set; }
        [Column(TypeName = "varchar(6)")]
        public string Tipo { get; set; }
        [Column(TypeName = "int")]
        public int Id_Usuario { get; set; }
        [Column(TypeName = "text")]
        public string Bkp { get; set; }
        [Column(TypeName = "int")]
        public int Id_Manifiesto_RS { get; set; }

    }
}
