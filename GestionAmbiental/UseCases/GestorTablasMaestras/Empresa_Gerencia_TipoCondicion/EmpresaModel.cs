using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.GestorTablasMaestras.Empresa_Gerencia_TipoCondicion
{
    public class EmpresaModel
    {        
        public int IdEmpresa { get; set; }
        [DisplayName("Razón Social:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe completar este campo")]
        public string RazonSocial { get; set; }
        [DisplayName("Estado:")]
        public string Estado { get; set; }

    }
}
