using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Domain.SGA_Documentos
{
    public class CategoriaTipo
    {
        public int IdSGACategoriaTipo { get; set; }
        public CategoriaDocumento CategoriaDocumento { get; set; }
        public TipoDocumento TipoDocumento { get; set; }
        public string Estado { get; set; }
    }
}
