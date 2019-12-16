using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.GestorTablasMaestras.Empresa_Gerencia_TipoCondicion
{
    public class TipoCondicionModel
    {
        public int IdTipoCondicion { get; set; }
        [DisplayName("Tipo Acto/Condición Subestándar:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe completar este campo")]
        public string Descripcion { get; set; }
        [DisplayName("Clasificación:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe seleccionar una opcción")]
        public string Grupo { get; set; }
    }
}
