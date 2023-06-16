using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GestionAmbiental.UseCases.GestionFOCAM
{
    public class GestionFOCAMModel
    {
        [Key]
        public int Id { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe completar este campo")]
        public int IdTipo { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe completar este campo")]
        public string Codigo { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe completar este campo")]
        public string Titulo { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe completar este campo")]
        public string Version { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe completar este campo")]
        public string FechaAprobacion { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe completar este campo")]
        public string Estado { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe completar este campo")]
        public string Comentarios { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe completar este campo")]
        public string FechaTentaivaVersion { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe completar este campo")]
        public string UrlDocumento { get; set; }


    }
}
