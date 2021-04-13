using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.GestorTablasMaestras.Gestiona
{
    public class FuentesTextoModel
    {
        public int Id { get; set; }
        [DisplayName("Fuente de Texto:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe seleccionar una opción")]
        public string Descripcion { get; set; }
    }
}
