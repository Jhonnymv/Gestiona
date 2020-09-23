using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities.ConsumoAgua
{
    [Table("Tb_Fuente_Agua")]
    public class FuenteAgua
    {
        [Key]
        public int Id_Fuente_Agua { get; set; }
        [Column(TypeName = "int")]
        public int Id_Origen_Agua { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Descripcion { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string Coordenadas_UTM { get; set; }
        [Column(TypeName = "varchar(3)")]
        public string Estado { get; set; }
    }
}
