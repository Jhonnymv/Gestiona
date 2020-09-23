using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities.ConsumoAgua
{
    [Table("Tb_Origen_Agua")]
    public class OrigenAgua
    {
        [Key]
        public int Id_Origen_Agua { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Descripcion { get; set; }
        [Column(TypeName = "varchar(3)")]
        public string Estado { get; set; }
    }
}
