using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.GestorTablasMaestras.SGA_Documentos.Model
{
    public class TipoDocumentoModel
    {
        public int IdSGATipoDocumento { get; set; }
        public string Descripcion { get; set; }
        public string Codigo { get; set; }
        public string Formato { get; set; }
        public string Estado { get; set; }
    }
}
