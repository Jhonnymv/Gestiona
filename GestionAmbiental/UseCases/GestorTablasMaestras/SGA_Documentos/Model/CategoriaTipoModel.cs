using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.GestorTablasMaestras.SGA_Documentos.Model
{
    public class CategoriaTipoModel
    {
        public int IdSGACategoriaTipo { get; set; }
        public int IdCategoriaDocumento { get; set; }
        public string CategoriaDocumento { get; set; }
        public int IdTipoDocumento { get; set; }
        public string TipoDocumento { get; set; }
        public string Estado { get; set; }
    }
}
