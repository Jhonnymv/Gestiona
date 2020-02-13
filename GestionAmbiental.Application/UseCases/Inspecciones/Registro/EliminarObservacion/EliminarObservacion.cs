using GestionAmbiental.Application.Repositories.GestorUsuarios;
using GestionAmbiental.Application.Repositories.Inspecciones;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Inspecciones.Registro.EliminarObservacion
{
    public class EliminarObservacion : IEliminarObservacion
    {
        private readonly IEditarInspeccionReadOnlyRepository _editarInspeccionReadOnlyRepository;
        private readonly IGestorUsuarioReadOnlyRepository _gestorUsuarioReadOnlyRepository;

        public EliminarObservacion(IEditarInspeccionReadOnlyRepository editarInspeccionReadOnlyRepository, IGestorUsuarioReadOnlyRepository gestorUsuarioReadOnlyRepository)
        {
            _editarInspeccionReadOnlyRepository = editarInspeccionReadOnlyRepository ?? throw new ArgumentNullException(nameof(editarInspeccionReadOnlyRepository));
            _gestorUsuarioReadOnlyRepository = gestorUsuarioReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorUsuarioReadOnlyRepository));
        }

        public async Task Execute(int IdObservacion, string usuario_red)
        {
            var usuario = await _gestorUsuarioReadOnlyRepository.GetUsuario(usuario_red);
            await _editarInspeccionReadOnlyRepository.EliminarInspeccionDetalle(IdObservacion, usuario.IdUsuario);
        }
    }
}
