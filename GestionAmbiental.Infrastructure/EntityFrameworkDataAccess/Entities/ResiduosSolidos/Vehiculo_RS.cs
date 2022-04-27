using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities.ResiduosSolidos
{
    [Table("Tb_Vehiculo_RS")]
    public class Vehiculo_RS
    {
        [Key]
        public int Id_Vehiculo_RS { get; set; }
        [Column(TypeName ="varchar(7)")]
        public string Placa { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public double Capacidad { get; set; }
        [Column(TypeName = "varchar(3)")]
        public string Estado { get; set; }
    }
}