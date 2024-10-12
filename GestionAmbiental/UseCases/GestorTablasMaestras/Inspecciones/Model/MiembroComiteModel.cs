using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GestionAmbiental.UseCases.GestorTablasMaestras.Inspecciones.Model
{
    public class MiembroComiteModel
    {
        public int IdMiembroComite { get; set; }
        [DisplayName("Nombres:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe completar este campo")]
        public string Nombres { get; set; }
        [DisplayName("Apellidos:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe completar este campo")]
        public string Apellidos { get; set; }
        public string Estado { get; set; }
    }
}
