using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities.AvistamientoFauna
{
    [Table("Tb_Lugar_Referencia_AF")]
    public class Lugar_Referencia_AF
    {
        [Key]
        public int Id_Lugar_Referencia_AF { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Descripcion { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string Latitud { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string Longitud { get; set; }
        [Column(TypeName = "varchar(3)")]
        public string Estado { get; set; }
    }
}
