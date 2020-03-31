using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities.Incidentes.Items
{
    [Table("Tb_Tipo_Incidente")]
    public class Componente_Ambiental
    {
        [Key]
        public int Id_Componente_Ambiental { get; set; }
        [Column(TypeName = "varchar(25)")]
        public string Descripcion { get; set; }
        [Column(TypeName = "varchar(3)")]
        public string Estado { get; set; }
    }
}
