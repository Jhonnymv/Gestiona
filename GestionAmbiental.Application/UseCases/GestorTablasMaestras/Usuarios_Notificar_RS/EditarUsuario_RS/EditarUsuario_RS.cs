using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Usuarios_Notificar_RS.EditarUsuario_RS
{
    public class EditarUsuario_RS : IEditarUsuario_RS
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public EditarUsuario_RS(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task Execute(int idUsuario, string nombres, string apellidos, int idRol, string email, string usuarioRed, string estado)
        {
            var usuario = new Domain.ValueObjects.Usuario_Notificar_RS();
            usuario.Id_Usuario_Notificar_RS = idUsuario;
            usuario.Nombres = nombres;
            usuario.Apellidos = apellidos;
            usuario.Id_Rol = idRol;
            usuario.Email = email;
            usuario.usuario_red = usuarioRed;
            usuario.Estado = estado;
            await _gestorTablasMaestrasReadOnlyRepository.UpdateUsuario_Notificar_RS(usuario);
        }
    }
}
