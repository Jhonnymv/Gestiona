using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Usuarios_Notificar_Incidente.EditarUsuarioIncidente
{
    public class EditarUsuarioIncidente : IEditarUsuarioIncidente
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public EditarUsuarioIncidente(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task Execute(int idUsuario, string nombres, string apellidos, int idRol, string email, string usuarioRed, string estado)
        {
            var usuario = new Domain.Incidentes.Items.Usuario_Notificar_Incidente();
            usuario.Id_Usuario_Notificar_Incidente = idUsuario;
            usuario.Nombres = nombres;
            usuario.Apellidos = apellidos;
            usuario.Id_Rol = idRol;
            usuario.Email = email;
            usuario.Usuario_Red = usuarioRed;
            usuario.Estado = estado;
            await _gestorTablasMaestrasReadOnlyRepository.UpdateUsuario_Notificar_Incidente(usuario);
        }
    }
}
