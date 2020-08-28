using GestionAmbiental.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Usuarios_Notificar_Avistamiento_Fauna
{
    public class UsuarioOutput
    {
        public int IdUsuario { get; set; }
        public int IdRol { get; set; }
        public string Rol { get; set; }
        public string UsuarioRed { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Email { get; set; }
        public string Superior { get; set; }
        public string Estado { get; set; }

        public UsuarioOutput()
        {
        }
        public UsuarioOutput(Domain.AvistamientoFauna.Items.Usuario_Notificar_Avistamiento_Fauna usuario)
        {
            IdUsuario = usuario.Id_Usuario_Notificar_Avistamiento_Fauna;
            IdRol = usuario.Id_Rol;
            Rol = ((InspeccionRol)usuario.Id_Rol).ToString();
            UsuarioRed = usuario.Usuario_Red;
            Nombres = usuario.Nombres;
            Apellidos = usuario.Apellidos;
            Email = usuario.Email;
            Estado = usuario.Estado;
        }
    }
}
