using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Usuarios_Notificar_Avistamiento_Fauna.EditarUsuario
{
    public class EditarUsuario : IEditarUsuario
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public EditarUsuario(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task Execute(int idUsuario, string nombres, string apellidos, int idRol, string email, string usuarioRed, string estado)
        {
            var usuario = new Domain.AvistamientoFauna.Items.Usuario_Notificar_Avistamiento_Fauna();
            usuario.Id_Usuario_Notificar_Avistamiento_Fauna = idUsuario;
            usuario.Nombres = nombres;
            usuario.Apellidos = apellidos;
            usuario.Id_Rol = idRol;
            usuario.Email = email;
            usuario.Usuario_Red = usuarioRed;
            usuario.Estado = estado;
            await _gestorTablasMaestrasReadOnlyRepository.UpdateUsuario_Notificar_AF(usuario);
        }
    }
}
