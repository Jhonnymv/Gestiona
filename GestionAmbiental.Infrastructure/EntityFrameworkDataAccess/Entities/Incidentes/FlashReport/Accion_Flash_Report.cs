using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities.Incidentes.FlashReport
{
    [Table("Tb_Accion_Flash_Report")]
    public class Accion_Flash_Report
    {
        [Key]
        public int Id_Accion_Flash_Report { get; set; }
        [Column(TypeName = "int")]
        public int Id_Flash_Report { get; set; }
        [Column(TypeName = "varchar(1100)")]
        public string Descripcion { get; set; }
        [Column(TypeName = "int")]
        public int Id_Usuario { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Responsable { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime Fecha { get; set; }
        [Column(TypeName = "varchar(10)")]
        public string Estatus { get; set; }
        [Column(TypeName = "varchar(15)")]
        public string Estado { get; set; }
    }
}
