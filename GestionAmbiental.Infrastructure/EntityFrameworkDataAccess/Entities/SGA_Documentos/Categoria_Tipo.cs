using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities.SGA_Documentos
{
    [Table("Tb_SGA_Categoria_Tipo")]
    public class Categoria_Tipo
    {
        [Key]
        public int Id_SGA_Categoria_Tipo { get; set; }
        [Column(TypeName = "int")]
        public int Id_SGA_Categoria_Documento { get; set; }
        [Column(TypeName = "int")]
        public int Id_SGA_Tipo_Documento { get; set; }
        [Column(TypeName = "bit")]
        public bool Visible { get; set; }
        [Column(TypeName = "varchar(3)")]
        public string Estado { get; set; }
    }
}
