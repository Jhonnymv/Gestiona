using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities.ResiduosSolidos
{
    [Table("Tb_Ingreso_Salida_RS")]
    public class Ingreso_Salida_RS
    {
        [Key]
        public int Id_Ingreso_Salida_RS { get; set; }
        [Column(TypeName = "int")]
        public int Id_ACR_CMRS { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Responsable_ACR_CMRS { get; set; }
        [Column(TypeName = "int")]
        public int Id_Ingreso_Salida { get; set; }
        [Column(TypeName = "int")]
        public int Id_Motivo { get; set; }
        public DateTime Fecha_Ingreso_Salida { get; set; }
        [Column(TypeName = "int")]
        public int Id_Residuo_Solido { get; set; }
        [Column(TypeName = "decimal(6,2)")]
        public double Cantidad { get; set; }
        [Column(TypeName = "int")]
        public int Id_Empresa_RS { get; set; }
        [Column(TypeName = "int")]
        public int Id_Responsable_Marcobre { get; set; }
        [Column(TypeName = "text")]
        public string Comentario { get; set; }
        [Column(TypeName = "varchar(10)")]
        public string Estado { get; set; }
    }
}
