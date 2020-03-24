using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.GestorTablasMaestras.ResiduosSolidos.ControlManifiestoRS
{
    public class UsuarioModel
    {

        [DisplayName("Usuarios:")]
        public int Id_Usuario_Notificar_RS { get; set; }
        [DisplayName("Rol:")]
        public int Id_Rol { get; set; }
        [DisplayName("Nombres:")]
        public string Nombres { get; set; }
        [DisplayName("Apellidos:")]
        public string Apellidos { get; set; }
        [DisplayName("Usuario de Red:")]
        public string usuario_red { get; set; }
        [DisplayName("Email:")]
        public string Email { get; set; }
        [DisplayName("Estado:")]
        public string Estado { get; set; }
    }
}
