using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Domain.SGA_Documentos
{
    public class Usuarios_Notificar_SGA
    {
        public int Id_Usuarios_Notificar_SGA { get; set; }
        public int Id_Rol { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Usuario_Red { get; set; }
        public string Email { get; set; }
        public string Estado { get; set; }
        public Usuarios_Notificar_SGA() { }

        public Usuarios_Notificar_SGA(int id_Usuarios_Notificar_SGA, int id_Rol, string nombres, string apellidos, string usuario_Red, string email, string estado)
        {
            Id_Usuarios_Notificar_SGA = id_Usuarios_Notificar_SGA;
            Id_Rol = id_Rol;
            Nombres = nombres;
            Apellidos = apellidos;
            Usuario_Red = usuario_Red;
            Email = email;
            Estado = estado;
        }
    }
}
