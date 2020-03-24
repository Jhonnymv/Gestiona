using GestionAmbiental.Application.Repositories.GestorUsuarios;
using GestionAmbiental.Application.Repositories.RAC;
using GestionAmbiental.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.RAC.Registro.EliminarRAC
{
    public class EliminarRAC : IEliminarRAC
    {
        private readonly IEditarObservacionRACReadOnlyRepository _editarObservacionRACReadOnlyRepository;
        private readonly IGestorUsuarioReadOnlyRepository _gestorUsuarioReadOnlyRepository;

        public EliminarRAC(IEditarObservacionRACReadOnlyRepository editarObservacionRACReadOnlyRepository, IGestorUsuarioReadOnlyRepository gestorUsuarioReadOnlyRepository)
        {
            _editarObservacionRACReadOnlyRepository = editarObservacionRACReadOnlyRepository ?? throw new ArgumentNullException(nameof(editarObservacionRACReadOnlyRepository));
            _gestorUsuarioReadOnlyRepository = gestorUsuarioReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorUsuarioReadOnlyRepository));
        }

        public async Task Execute(int idRAC, string usuario_red)
        {
            var usuario = await _gestorUsuarioReadOnlyRepository.GetUsuario(usuario_red);
            if(usuario.IdRol.Equals((int)InspeccionRol.Admin))
                await _editarObservacionRACReadOnlyRepository.EliminarRAC(idRAC, usuario.IdUsuario);
        }
    }
}
