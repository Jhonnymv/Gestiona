using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.GestorTablasMaestras.ResiduosSolidos.ControlIngresoSalida
{
    public class ResiduosSolidosModel
    {
        public int Id_Residuo_Solido { get; set; }
        [DisplayName("Residuo Sólido:")]
        [Range(1, 99, ErrorMessage = "Debe seleccionar una opción")]
        public int Id_Tipo_Residuo { get; set; }
        [DisplayName("Tipo de Residuo:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe completar este campo")]
        public string Descripcion { get; set; }
        [DisplayName("Estado:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe seleccionar una opción")]
        public string Estado { get; set; }
    }
}
