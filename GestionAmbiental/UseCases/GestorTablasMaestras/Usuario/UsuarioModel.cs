using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.GestorTablasMaestras.Usuario
{
    public class UsuarioModel
    {
        public int IdUsuario { get; set; }
        [DisplayName("Rol:")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Debe seleccionar una opcción")]
        public int IdRol { get; set; }
        [DisplayName("Usuario de Red:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe completar este campo")]
        public string UsuarioRed { get; set; }
        [DisplayName("Nombres:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe completar este campo")]
        public string Nombres { get; set; }
        [DisplayName("Apellidos:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe completar este campo")]
        public string Apellidos { get; set; }
        [DisplayName("Email:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe completar este campo")]
        [EmailAddress(ErrorMessage ="Debe ingresar una cuenta de correo electrónico válida")]
        public string Email { get; set; }
        [DisplayName("Superior:")]
        public int? IdSuperior { get; set; }
        [DisplayName("Estado:")]
        public string Estado { get; set; }
    }
}
