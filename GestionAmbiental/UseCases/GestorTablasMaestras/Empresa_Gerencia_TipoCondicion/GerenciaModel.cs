using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.GestorTablasMaestras.Empresa_Gerencia_TipoCondicion
{
    public class GerenciaModel
    {
        public int IdGerencia { get; set; }
        [DisplayName("Gerencia:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe completar este campo")]
        public string Descripcion { get; set; }
        [DisplayName("Estado:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe completar este campo")]
        public string Estado { get; set; }
    }
}
