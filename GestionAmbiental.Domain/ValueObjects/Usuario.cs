using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Domain.ValueObjects
{
    public class Usuario
    {
        public int Id_Usuario { get; set; }
        public int Id_Rol { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Email { get; set; }

        public Usuario(int id_Usuario, int id_Rol, string nombres, string apellidos, string email)
        {
            Id_Usuario = id_Usuario;
            Id_Rol = id_Rol;
            Nombres = nombres;
            Apellidos = apellidos;
            Email = email;
        }

        public Usuario()
        {
        }
    }
}
