using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GestionAmbiental.UseCases.GestorTablasMaestras.Inspecciones.Model
{
    public class TipoInspeccionModel
    {
        public int IdTipoInspeccion { get; set; }
        [DisplayName("Descripcion:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe completar este campo")]
        public string Descripcion { get; set; }
    }
}
