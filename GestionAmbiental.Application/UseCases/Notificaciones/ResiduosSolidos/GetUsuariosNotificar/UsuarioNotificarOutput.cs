using GestionAmbiental.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.Notificaciones.ResiduosSolidos.GetUsuariosNotificar
{
    public class UsuarioNotificarOutput
    {
        public int Id_Usuario_Notificar_RS { get; set; }
        public int Id_Rol { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string usuario_red { get; set; }
        public string Email { get; set; }
        public string Estado { get; set; }
        public UsuarioNotificarOutput(Usuario_Notificar_RS usuario)
        {
            Id_Usuario_Notificar_RS = usuario.Id_Usuario_Notificar_RS;
            Id_Rol = usuario.Id_Rol;
            Nombres = usuario.Nombres;
            Apellidos = usuario.Apellidos;
            usuario_red = usuario.usuario_red;
            Email = usuario.Email;
            Estado = usuario.Estado;
        }
    }
}
