using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GestionAmbiental.UseCases.MonitoreoCampoInterno.CalidadAgua
{
    public class CalidadAguaModel
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Ubicación:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe completar este campo")]
        public string Ubicacion { get; set; }
     
        
    }
}
 