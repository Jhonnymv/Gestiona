using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Domain.ValueObjects
{
    public class Usuario_Notificar_RS
    {
        public int Id_Usuario_Notificar_RS { get; set; }
        public int Id_Rol { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string usuario_red { get; set; }
        public string Email { get; set; }
        public string Estado { get; set; }

        public Usuario_Notificar_RS() { }
        public Usuario_Notificar_RS(int id_Usuario_Notificar_RS, int id_Rol, string nombres, string apellidos, string usuario_red, string email, string estado)
        {
            Id_Usuario_Notificar_RS = id_Usuario_Notificar_RS;
            Id_Rol = id_Rol;
            Nombres = nombres;
            Apellidos = apellidos;
            this.usuario_red = usuario_red;
            Email = email;
            Estado = estado;
        }
    }
}
