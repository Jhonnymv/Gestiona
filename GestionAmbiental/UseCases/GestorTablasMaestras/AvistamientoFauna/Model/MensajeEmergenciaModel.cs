using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.GestorTablasMaestras.AvistamientoFauna.Model
{
    public class MensajeEmergenciaModel
    {

        [DisplayName("Mensaje:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe completar este campo")]
        public string Mensaje { get; set; }
    }
}
