using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.SGA_Documentos.TipoDocumento
{
    public class TipoDocumentoOutput
    {
        public int IdSGATipoDocumento { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public string Formato { get; set; }
        public string Estado { get; set; }
        public TipoDocumentoOutput() { }
        public TipoDocumentoOutput(Domain.SGA_Documentos.TipoDocumento tipoDocumento) 
        {
            IdSGATipoDocumento = tipoDocumento.IdSGATipoDocumento;
            Codigo = tipoDocumento.Codigo;
            Descripcion = tipoDocumento.Descripcion;
            Formato = tipoDocumento.Formato;
            Estado = tipoDocumento.Estado;
        }
    }
}
