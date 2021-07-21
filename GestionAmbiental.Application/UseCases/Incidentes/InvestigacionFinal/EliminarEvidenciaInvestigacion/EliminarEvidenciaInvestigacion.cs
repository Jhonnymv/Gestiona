using GestionAmbiental.Application.Repositories.Incidentes.InvestigacionFinal;
using GestionAmbiental.Application.UseCases.GestorUsuarios.GetUsuario;
using GestionAmbiental.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.EliminarEvidenciaInvestigacion
{
    public class EliminarEvidenciaInvestigacion : IEliminarEvidenciaInvestigacion
    {
        private readonly IEditarInvestigacionFinalReadOnlyRepository _editarInvestigacionFinalReadOnlyRepository;
        private readonly IGetInvestigacionFinalReadOnlyRepository _getInvestigacionFinalReadOnlyRepository;
        private readonly IGetUsuario _getUsuario;

        public EliminarEvidenciaInvestigacion(IEditarInvestigacionFinalReadOnlyRepository editarInvestigacionFinalReadOnlyRepository, IGetInvestigacionFinalReadOnlyRepository getInvestigacionFinalReadOnlyRepository, IGetUsuario getUsuario)
        {
            _editarInvestigacionFinalReadOnlyRepository = editarInvestigacionFinalReadOnlyRepository ?? throw new ArgumentNullException(nameof(editarInvestigacionFinalReadOnlyRepository));
            _getInvestigacionFinalReadOnlyRepository = getInvestigacionFinalReadOnlyRepository ?? throw new ArgumentNullException(nameof(getInvestigacionFinalReadOnlyRepository));
            _getUsuario = getUsuario ?? throw new ArgumentNullException(nameof(getUsuario));
        }

        public async Task Execute(int IdInvestigacion, string Tipo, string NombreArchivo,string usuario_red)
        {
            var investigacion = await _getInvestigacionFinalReadOnlyRepository.GetInvestigacionFinal(IdInvestigacion);
            var usuario = await _getUsuario.Execute(usuario_red);
            if(investigacion.Supervisor_MA.IdUsuario.Equals(usuario.IdUsuario) || usuario.IdRol.Equals((int)InspeccionRol.Admin))
                await _editarInvestigacionFinalReadOnlyRepository.EliminarEvidenciaInvestigacion(IdInvestigacion, Tipo, NombreArchivo);
        }
    }
}
