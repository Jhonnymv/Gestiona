using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.SGA_Documentos
{
    public class SGADocumentoModel
    {
        public int Id_SGA_Documento { get; set; }
        [DisplayName("Tipo de Documento:")]
        public int Id_SGA_Tipo_Documento { get; set; }
        [DisplayName("Código:")]
        public string Doc_Codigo { get; set; }        
        [DisplayName("Nombre:")]
        public string Doc_Nombre { get; set; }
        [DisplayName("Versión:")]
        public double Doc_Version { get; set; }
        [DisplayName("Predecesor:")]
        public int? Doc_Predecesor { get; set; }       
        [DisplayName("Condición:")]
        public int Condicion { get; set; }
        [DisplayName("Comentario:")]
        public string Comentario { get; set; }
        [DisplayName("Documento:")]
        public IFormFile Documento { get; set; }
    }
}
