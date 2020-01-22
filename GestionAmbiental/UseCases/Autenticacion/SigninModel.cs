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
        [Required(ErrorMessage ="Debe ingresar su nombre de usuario")]
        [DisplayName("Usuario")]
        public string User { get; set; }
        [Required(ErrorMessage = "Debe ingresar su contraseña")]
        [DisplayName("Contraseña")]
        public string Password { get; set; }
    }
}
