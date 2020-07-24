using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities.AvistamientoFauna
{
    [Table("Tb_Nombre_Comun_Fauna")]
    public class Nombre_Comun_Fauna
    {
        [Key]
        public int Id_Nombre_Comun_Fauna { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Descripcion { get; set; }
        [Column(TypeName = "int")]
        public int Id_Tipo_Fauna { get; set; }
        [Column(TypeName = "text")]
        public string Foto { get; set; }
        [Column(TypeName = "varchar(3)")]
        public string Estado { get; set; }
    }
}
