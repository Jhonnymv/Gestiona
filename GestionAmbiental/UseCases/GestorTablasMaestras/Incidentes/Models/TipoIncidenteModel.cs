using GestionAmbiental;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.GestorTablasMaestras.Incidentes.Models
{
    public class TipoIncidenteModel
    {
        public int? Id_Tipo_Incidente { get; set; }
        [DisplayName("Descripción:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe completar este campo")]
        public string Descripcion { get; set; }
        [DisplayName("Estado:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe completar este campo")]
        public string Estado { get; set; }
    }
}
