using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities.FOCAM
{
    [Table("Tb_FOCAM_Documentos")]
    public class Documentos
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "int")]
        public int Id_Tipo { get; set; }

        [Column(TypeName = "varhar(255)")]
        public string Codigo { get; set; }

        [Column(TypeName = "varhar(255)")]
        public string Titulo { get; set; }
        
        [Column(TypeName = "varhar(255)")]
        public string Version { get; set; }
      
        [Column(TypeName = "Date")]
        public DateTime FechaAprobacion { get; set; }

        [Column(TypeName = "varhar(255)")]
        public string Estado { get; set; }

        [Column(TypeName = "Text")]
        public string Comentarios { get; set; }

        [Column(TypeName = "Date")]
        public DateTime FechaTentaivaVersion { get; set; }

        [Column(TypeName = "Text")]
        public string UrlDocumento { get; set; }
    }
}
