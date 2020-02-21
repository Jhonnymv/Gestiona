using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;


namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities.Inspecciones
{
    [Table("Tb_Log_Inspeccion")]
    public class Log_Inspeccion
    {
        [Key]
        public int Id_Log_Inspeccion { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime Fecha { get; set; }
        [Column(TypeName = "varchar(6)")]
        public string Tipo { get; set; }
        [Column(TypeName = "int")]
        public int Id_Usuario { get; set; }
        [Column(TypeName = "text")]
        public string Bkp { get; set; }
        [Column(TypeName = "int")]
        public int Id_Inspeccion_Cabecera { get; set; }
    }
}
