using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Domain.ValueObjects
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public int IdRol { get; set; }
        public string UsuarioRed { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Email { get; set; }
        public int? IdSuperior { get; set; }
        public string Estado { get; set; }

        public Usuario(int id_Usuario, int id_Rol, string nombres, string apellidos, string email)
        {
            IdUsuario = id_Usuario;
            IdRol = id_Rol;
            Nombres = nombres.ToUpper();
            Apellidos = apellidos.ToUpper();
            Email = email;
        }

        public Usuario(int idUsuario, int idRol, string usuarioRed, string nombres, string apellidos, string email, int? idSuperior, string estado)
        {
            IdUsuario = idUsuario;
            IdRol = idRol;
            Nombres = nombres.ToUpper();
            Apellidos = apellidos.ToUpper();
            Email = email;
            UsuarioRed = usuarioRed;
            IdSuperior = idSuperior;
            Estado = estado;
        }
        public Usuario()
        {
        }
    }
}
