using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities.ConsumoAgua
{
    [Table("Tb_Consumo_Agua")]
    public class ConsumoAgua
    {
        [Key]
        public int Id_Consumo_Agua { get; set; }
        [Column(TypeName = "int")]
        public int Id_Resolucion_Consumo_Agua { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime Fecha { get; set; }
        [Column(TypeName = "decimal(5,2)")]
        public double Tiempo_Bombeo { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public double Lectura_Inicial { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public double Lectura_Final { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public double Volumen_Bombeado { get; set; }
        [Column(TypeName = "decimal(5,2)")]
        public double Caudal_Promedio { get; set; }
        [Column(TypeName = "int")]
        public int Id_Usuario { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime Fecha_Registro { get; set; }
        [Column(TypeName = "varchar(30)")]
        public string Estado { get; set; }

    }
}
