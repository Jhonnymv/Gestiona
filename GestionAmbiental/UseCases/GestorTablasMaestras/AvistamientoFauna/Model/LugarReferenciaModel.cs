using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.GestorTablasMaestras.AvistamientoFauna.Model
{
    public class LugarReferenciaModel
    {
        public int IdLugarReferencia { get; set; }
        [DisplayName("Descripción:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe completar este campo")]
        public string Descripcion { get; set; }
        [DisplayName("Latitud:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe completar este campo")]
        public string Latitud { get; set; }
        [DisplayName("Longitud:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe completar este campo")]
        public string Longitud { get; set; }
        [DisplayName("Estado:")]
        public string Estado { get; set; }
    }
}
