using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities.SGA_Documentos
{
    [Table("Tb_SGA_Documento")]
    public class Documento
    {
        [Key]
        public int Id_SGA_Documento { get; set; }
        public int Id_SGA_Categoria_Tipo { get; set; }
        [Column(TypeName = "varchar(30)")]
        public string Doc_Codigo { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Doc_Nombre { get; set; }
        [Column(TypeName = "decimal(5,2)")]
        public double Doc_Version { get; set; }
        [Column(TypeName = "bit")]
        public bool Principal { get; set; }
        [Column(TypeName = "int")]
        public int? Doc_Predecesor { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Fecha_Aprobacion { get; set; }
        [Column(TypeName = "int")]
        public int Condicion { get; set; }
        [Column(TypeName = "text")]
        public string Comentario { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Fecha_Tentativa { get; set; }
        [Column(TypeName = "text")]
        public string Pdf_Ruta { get; set; }
        [Column(TypeName = "text")]
        public string Editable_Ruta { get; set; }
        [Column(TypeName = "int")]
        public int Id_Usuario { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime Fecha_Creacion { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime Fecha_Actualizacion { get; set; }
        [Column(TypeName = "varchar(30)")]
        public string Estado { get; set; }
    }
}
