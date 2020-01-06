using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.GestorTablasMaestras.Info_SGA
{
    public class Info_SGAModel
    {
        public int IdInfo { get; set; }
        [DisplayName("Información (JSON):")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe completar este campo")]
        public string Info { get; set; }
        [DisplayName("Tipo de Información:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe completar este campo")]
        public string Tipo { get; set; }
    }
}
