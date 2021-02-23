using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities.SGA_Documentos
{
    [Table("Tb_SGA_Documento")]
    public class Documentos_Relacionados
    {
        [Key]
        public int Id_SGA_Documentos_Relacionados { get; set; }
        [Column(TypeName = "int")]
        public int Id_Documento_Principal { get; set; }
        [Column(TypeName = "int")]
        public int Id_Documento_Secundario { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime Fecha_Relacion { get; set; }
        [Column(TypeName = "varchar(3)")]
        public string Estado { get; set; }
    }
}
