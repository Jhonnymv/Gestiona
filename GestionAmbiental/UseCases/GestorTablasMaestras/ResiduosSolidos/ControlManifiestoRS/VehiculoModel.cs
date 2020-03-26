using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.GestorTablasMaestras.ResiduosSolidos.ControlManifiestoRS
{
    public class VehiculoModel
    {
        public int Id_Vehiculo_RS { get; set; }
        [DisplayName("Placa:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe completar este campo")]
        public string Placa { get; set; }
        [DisplayName("Capacidad del Vehículo (TN):")]
        [Range(1, 99, ErrorMessage = "Debe seleccionar una opción")]
        public double Capacidad { get; set; }
        [DisplayName("Estado:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe completar este campo")]
        public string Estado { get; set; }
    }
}
