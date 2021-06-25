using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.SGA_Documentos
{
    public class FormatoDocumentoModel
    {
        public int? Id_Documento_Principal{ get; set; }
        [DisplayName("Tipo de Documento:")]
        public int Id_Tipo_Documento_Formato { get; set; }
        [DisplayName("Elaborado Por:")]
        public int Id_Elaborado_Por { get; set; }
        [DisplayName("Nombre:")]
        public string Doc_Nombre_Formato { get; set; }
        [DisplayName("Versión:")]
        public double Doc_Version_Formato { get; set; }
        [DisplayName("Fecha Tentativa de la Nueva Versión:")]
        public string Fecha_Tentativa { get; set; }
        [DisplayName("Fecha de Aprobación:")]
        public string Fecha_Aprobacion { get; set; }
        [DisplayName("Comentario:")]
        public string Comentario_Formato { get; set; }
        [DisplayName("Documento Pdf:")]
        public IList<IFormFile> Documento_Formato_Pdf { get; set; }
        [DisplayName("Documento Editable:")]
        public IList<IFormFile> Documento_Formato_Editable { get; set; }
    }
}
