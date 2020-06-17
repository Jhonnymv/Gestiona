using GestionAmbiental;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.GestorTablasMaestras.Incidentes.Models
{
    public class UnidadMedidaModel
    {
        public int Id_Unidad_Medida { get; set; }
        [DisplayName("Descripción:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe completar este campo")]
        public string Descripcion { get; set; }
        [DisplayName("Tipo:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe completar este campo")]
        public string Tipo { get; set; }
        [DisplayName("Estado:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe completar este campo")]
        public string Estado { get; set; }
    }
}
