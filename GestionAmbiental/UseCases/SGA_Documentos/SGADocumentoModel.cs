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
        public int Id_SGA_Documento { get; set; }
        public string Id_Encriptado { get; set; }
        public int Doc_Principal { get; set; }
        [DisplayName("Tipo de Documento:")]
        public int Id_SGA_Tipo_Documento { get; set; }
        public int Id_Categoria { get; set; }
        [DisplayName("Nombre:")]
        public string Doc_Nombre { get; set; }
        [DisplayName("Código:")]
        public string Doc_Codigo { get; set; }
        [DisplayName("Versión:")]
        public double Doc_Version { get; set; }
        [DisplayName("Predecesor:")]
        public int? Doc_Predecesor { get; set; }
        [DisplayName("Estado:")]
        public int Condicion { get; set; }
        [DisplayName("Fecha Tentaiva de la Nueva Versión:")]
        public string Fecha_Tentativa { get; set; }
        [DisplayName("Comentario:")]
        public string Comentario { get; set; }
        [DisplayName("Documento:")]
        public IFormFile Documento { get; set; }
        public string DocumentoRuta { get; set; }
        public string Opcion { get; set; }
        public string Revisores {get;set;}
        public string Aprobadores {get;set;}
        public string Secundarios {get;set;}
    }
}
