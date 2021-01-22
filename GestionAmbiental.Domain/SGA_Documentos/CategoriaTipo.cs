using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Domain.SGA_Documentos
{
    public class CategoriaTipo
    {
        public int IdSGACategoriaTipo { get; set; }
        public int IdSGACategoriaDocumento { get; set; }
        public int IdSGATipoDocumento { get; set; }
        public bool Visible { get; set; }
        public string Estado { get; set; }
    }
}
