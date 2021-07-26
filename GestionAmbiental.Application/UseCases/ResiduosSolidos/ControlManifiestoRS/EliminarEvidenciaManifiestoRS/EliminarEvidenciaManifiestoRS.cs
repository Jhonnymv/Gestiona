using GestionAmbiental.Application.Repositories.ResiduosSolidos.ControlManifiestoRS;
using GestionAmbiental.Application.UseCases.GestorUsuarios.GetUsuario;
using GestionAmbiental.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.ResiduosSolidos.ControlManifiestoRS.EliminarEvidenciaManifiestoRS
{
    public class EliminarEvidenciaManifiestoRS : IEliminarEvidenciaManifiestoRS
    {
        private readonly IEditarManifiestoRSReadOnlyRepository _editarManifiestoRSReadOnlyRepository; 
        private readonly IGetUsuario _getUsuario;

        public EliminarEvidenciaManifiestoRS(IEditarManifiestoRSReadOnlyRepository editarManifiestoRSReadOnlyRepository, IGetUsuario getUsuario)
        {
            _editarManifiestoRSReadOnlyRepository = editarManifiestoRSReadOnlyRepository ?? throw new ArgumentNullException(nameof(editarManifiestoRSReadOnlyRepository));
            _getUsuario = getUsuario ?? throw new ArgumentNullException(nameof(getUsuario));
        }

        public async Task Execute(int IdManifiesto, string Tipo, string NombreArchivo, string usuarioRed)
        {
            var usuario = await _getUsuario.Execute(usuarioRed);
            if (usuario.IdRol.Equals((int)InspeccionRol.Admin))
                await _editarManifiestoRSReadOnlyRepository.EliminarEvidenciaInvestigacion(IdManifiesto, Tipo, NombreArchivo);
        }
    }
}
