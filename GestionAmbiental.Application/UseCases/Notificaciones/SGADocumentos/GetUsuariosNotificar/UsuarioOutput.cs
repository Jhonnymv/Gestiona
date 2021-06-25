using GestionAmbiental.Domain.SGA_Documentos;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.Notificaciones.SGADocumentos.GetUsuariosNotificar
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
        public UsuarioOutput(Usuarios_Notificar_SGA usuario)
        {
            IdUsuario = usuario.Id_Usuarios_Notificar_SGA;
            IdRol = usuario.Id_Rol;
            UsuarioRed = usuario.Usuario_Red;
            Nombres = usuario.Nombres;
            Apellidos = usuario.Apellidos;
            Email = usuario.Email;
            Estado = usuario.Estado;
        }
    }
}
