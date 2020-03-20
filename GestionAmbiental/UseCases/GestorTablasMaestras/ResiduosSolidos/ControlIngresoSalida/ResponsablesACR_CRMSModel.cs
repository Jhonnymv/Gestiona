using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.GestorTablasMaestras.ResiduosSolidos.ControlIngresoSalida
{
    public class ResponsablesACR_CRMSModel
    {
        public int Id_Responsable_ACR_CRMS { get; set; }
        [DisplayName("Nombres:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe completar este campo")]
        public string Nombres { get; set; }
        [DisplayName("Apellidos:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe completar este campo")]
        public string Apellidos { get; set; }
        [DisplayName("Estado:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe completar este campo")]
        public string Estado { get; set; }
    }
}
