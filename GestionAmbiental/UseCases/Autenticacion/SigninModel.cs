using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.Autenticacion
{
    public class SigninModel
    {
        [Required(ErrorMessage ="Ambos campos son requeridos")]
        [DisplayName("Usuario")]
        public string User { get; set; }
        [Required(ErrorMessage = "Ambos campos son requeridos")]
        [DisplayName("Contraseña")]
        public string Password { get; set; }
        public string MensajeAut { get; set; }
    }
}
