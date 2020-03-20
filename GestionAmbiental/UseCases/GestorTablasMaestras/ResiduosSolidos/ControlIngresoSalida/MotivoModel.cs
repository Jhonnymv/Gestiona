using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.GestorTablasMaestras.ResiduosSolidos.ControlIngresoSalida
{
    public class MotivoModel
    {
        public int Id_Motivo { get; set; }
        [DisplayName("Motivo de Ingreso/Salida:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe completar este campo")]
        public string Descripcion { get; set; }
        [DisplayName("Estado:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe seleccionar una opción")]
        public string Estado { get; set; }
    }
}
