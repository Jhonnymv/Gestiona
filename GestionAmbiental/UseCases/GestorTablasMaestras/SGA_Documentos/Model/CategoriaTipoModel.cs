using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.GestorTablasMaestras.SGA_Documentos.Model
{
    public class CategoriaTipoModel
    {
        public int IdSGACategoriaTipo { get; set; }
        [DisplayName("Categoría de documento:")]
        [Range(0, Int32.MaxValue, ErrorMessage = "Debe seleccionar una opción")]
        public int IdCategoriaDocumento { get; set; }
        public string CategoriaDocumento { get; set; }
        [DisplayName("Tipo de documento:")]
        [Range(0, Int32.MaxValue, ErrorMessage = "Debe seleccionar una opción")]
        public int IdTipoDocumento { get; set; }
        public string TipoDocumento { get; set; }
        public string Estado { get; set; }
    }
}
