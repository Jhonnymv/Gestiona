using GestionAmbiental.Application.UseCases.SGA_Documentos;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.SGA_Documentos
{
    public class SGADocumentoModel
    {
        public int IdSGADocumento { get; set; }
        public string IdEncriptado { get; set; }
        public int DocPrincipal { get; set; }
        [DisplayName("Tipo de Documento")]
        public int IdSGATipoDocumento { get; set; }
        [DisplayName("Elaborado Por")]
        public int IdElaboradoPor { get; set; }
        public int IdCategoria { get; set; }
        [DisplayName("Nombre")]
        public string DocNombre { get; set; }
        [DisplayName("Código")]
        public string DocCodigo { get; set; }
        [DisplayName("Versión")]
        public double DocVersion { get; set; }
        [DisplayName("Predecesor")]
        public int? DocPredecesor { get; set; }
        [DisplayName("Estado")]
        public int Condicion { get; set; }
        [DisplayName("Fecha Tentativa de la Nueva Versión")]
        public string FechaTentativa { get; set; }
        [DisplayName("Fecha de Aprobación")]
        public string FechaAprobacion { get; set; }
        [DisplayName("Comentario")]
        public string Comentario { get; set; }
        [DisplayName("Documento PDF")]
        public IFormFile DocumentoPdf { get; set; }
        [DisplayName("Documento Editable")]
        public IFormFile DocumentoEditable { get; set; }
        public string DocPdfRuta { get; set; }
        public string DocEditableRuta { get; set; }
        public string Opcion { get; set; }
        public string Revisores {get;set;}
        public string Aprobadores {get;set;}
        public string Secundarios {get;set;}
    }
}
