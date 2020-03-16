using GestionAmbiental.Application.Repositories.GestorUsuarios;
using GestionAmbiental.Application.Repositories.ResiduosSolidos.ControlManifiestoRS;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.ResiduosSolidos.ControlManifiestoRS.EliminarManifiestoRS
{
    public class EliminarManifiestoRS : IEliminarManifiestoRS
    {
        private readonly IEditarManifiestoRSReadOnlyRepository _editarManifiestoRSReadOnlyRepository;
        private readonly IGestorUsuarioReadOnlyRepository _gestorUsuarioReadOnlyRepository;
        public EliminarManifiestoRS(IEditarManifiestoRSReadOnlyRepository editarManifiestoRSReadOnlyRepository, IGestorUsuarioReadOnlyRepository gestorUsuarioReadOnlyRepository)
        {
            _editarManifiestoRSReadOnlyRepository = editarManifiestoRSReadOnlyRepository ?? throw new ArgumentNullException(nameof(editarManifiestoRSReadOnlyRepository));
            _gestorUsuarioReadOnlyRepository = gestorUsuarioReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorUsuarioReadOnlyRepository));
        }
        public async Task Execute(int IdManifiestoRS, string usuario_red)
        {
            var id_usuario = await _gestorUsuarioReadOnlyRepository.GetIdUsuario(usuario_red);
            await _editarManifiestoRSReadOnlyRepository.EliminarManifiesto(IdManifiestoRS, id_usuario);
        }
    }
}
