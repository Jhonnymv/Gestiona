using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities.SGA_Documentos
{
    [Table("Tb_SGA_Revision_Documento")]
    public class Revision_Documento
    {
        [Key]
        public int Id_SGA_Revision_Documento { get; set; }
        [Column(TypeName = "int")]
        public int Id_SGA_Documento { get; set; }
        [Column(TypeName = "int")]
        public int Id_Usuario { get; set; }
        [Column(TypeName = "int")]
        public int Orden { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime Fecha_Limite { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Fecha_Revision { get; set; }
        [Column(TypeName = "varchar(30)")]
        public string Estado { get; set; }
    }
}
