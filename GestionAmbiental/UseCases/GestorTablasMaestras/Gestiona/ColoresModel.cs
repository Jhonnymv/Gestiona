using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.GestorTablasMaestras.Gestiona
{
    public class ColoresModel
    {
        public int Id { get; set; }

        [DisplayName("Descripción:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe completar este campo")]
        public string Descripcion { get; set; }
        [DisplayName("Variable CSS:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe completar este campo")]
        public string Variable { get; set; }
        [DisplayName("Color:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe seleccionar un color")]
        public string Valor { get; set; }
    }
}
