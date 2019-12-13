using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Usuario.EditarUsuario
{
    public class EditarUsuario : IEditarUsuario
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public EditarUsuario(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task Execute(int idUsuario, string nombres, string apellidos, int idRol, int? idSuperior, string email, string usuarioRed, string estado)
        {
            var usuario = new Domain.ValueObjects.Usuario();
            usuario.IdUsuario = idUsuario;
            usuario.Nombres = nombres;
            usuario.Apellidos = apellidos;
            usuario.IdRol = idRol;
            usuario.IdSuperior = idSuperior;
            usuario.Email = email;
            usuario.UsuarioRed = usuarioRed;
            usuario.Estado = estado;
            await _gestorTablasMaestrasReadOnlyRepository.UpdateUsuario(usuario);
        }
    }
}
