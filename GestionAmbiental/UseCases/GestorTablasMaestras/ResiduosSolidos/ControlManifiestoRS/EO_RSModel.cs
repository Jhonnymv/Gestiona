using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.GestorTablasMaestras.ResiduosSolidos.ControlManifiestoRS
{
    public class EO_RSModel
    {
        public int Id_EO_RS { get; set; }
        [DisplayName("Razón Social:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe completar este campo")]
        public string Razon_Social { get; set; }
        [DisplayName("Número de Registro:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe completar este campo")]
        public string Registro { get; set; }
        [DisplayName("Fecha de Vencimiento de Registro:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe completar este campo")]
        public string Fecha_Vencimiento_Registro { get; set; }
        [DisplayName("Estado:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe completar este campo")]
        public string Estado { get; set; }
    }
}
