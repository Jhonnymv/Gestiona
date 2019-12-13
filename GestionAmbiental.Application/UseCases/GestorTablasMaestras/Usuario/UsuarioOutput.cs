using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Usuario
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
        public int? IdSuperior { get; set; }
        public string Superior { get; set; }
        public string Estado { get; set; }

        public UsuarioOutput()
        {
        }
        public UsuarioOutput(Domain.ValueObjects.Usuario usuario)
        {
            IdUsuario = usuario.IdUsuario;
            IdRol = usuario.IdRol;
            UsuarioRed = usuario.UsuarioRed;
            Nombres = usuario.Nombres;
            Apellidos = usuario.Apellidos;
            Email = usuario.Email;
            IdSuperior = usuario.IdSuperior;
            Estado = usuario.Estado;
        }
    }
}
