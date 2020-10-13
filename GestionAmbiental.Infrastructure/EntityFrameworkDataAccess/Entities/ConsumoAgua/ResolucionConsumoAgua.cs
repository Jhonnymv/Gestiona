using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities.ConsumoAgua
{
    [Table("Tb_Resolucion_Consumo_Agua")]
    public class ResolucionConsumoAgua
    {
        [Key]
        public int Id_Resolucion_Consumo_Agua { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Resolucion { get; set; }
        [Column(TypeName = "varchar(4)")]
        public string Anio { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string Tipo_Uso { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Clase_Derecho { get; set; }
        [Column(TypeName = "int")]
        public int Id_Fuente_Agua { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public double Volumen_Otorgado { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string Actividad { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public double? Limite_Diario { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public double? Limite_Semanal { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public double? Limite_Mensual { get; set; }
        [Column(TypeName = "varchar(30)")]
        public string Estado { get; set; }
    }
}
