using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities.AvistamientoFauna
{
    [Table("Tb_Log_Avistamiento_Fauna")]
    public class Log_Avistamiento_Fauna
    {
        [Key]
        public int Id_Log_Avistamiento_Fauna { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime Fecha { get; set; }
        [Column(TypeName = "varchar(6)")]
        public string Tipo { get; set; }
        [Column(TypeName = "int")]
        public int Id_Usuario { get; set; }
        [Column(TypeName = "text")]
        public string Bkp { get; set; }
        [Column(TypeName = "int")]
        public int Id_Avistamiento_Fauna { get; set; }
    }
}
