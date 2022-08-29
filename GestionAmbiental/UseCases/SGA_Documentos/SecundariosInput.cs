using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.SGA_Documentos
{
    public class SecundariosInput
    {
        public string IdEncriptado { get; set; }
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string TipoDocumento { get; set; }
        public string Nombre { get; set; }
        public string Version { get; set; }
        public string Condicion { get; set; }
        public string RutaPdf { get; set; }
        public string RutaEditable { get; set; }
    }
}
