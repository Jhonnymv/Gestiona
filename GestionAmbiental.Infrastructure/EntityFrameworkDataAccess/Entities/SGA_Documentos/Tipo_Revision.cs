using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities.SGA_Documentos
{
    [Table("Tb_SGA_Tipo_Revision")]
    public class Tipo_Revision
    {
        [Key]
        public int Id_SGA_Tipo_Revision { get; set; }
        [Column(TypeName = "varchar(25)")]
        public string Descripcion { get; set; }
        [Column(TypeName = "varchar(3)")]
        public string Estado { get; set; }
    }
}
