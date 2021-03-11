using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.GestorTablasMaestras.SGA_Documentos.Model
{
    public class TipoDocumentoModel
    {
        public int IdSGATipoDocumento { get; set; }
        [DisplayName("Descripción:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe completar este campo")]
        public string Descripcion { get; set; }
        [DisplayName("Código:")]
        public string Codigo { get; set; }
        [DisplayName("Nuevo Formato:")]
        public IFormFile Formato { get; set; }
        [DisplayName("Formato Actual:")]
        public string RutaFormato { get; set; }
        [DisplayName("Estado:")]
        public string Estado { get; set; }
    }
}
