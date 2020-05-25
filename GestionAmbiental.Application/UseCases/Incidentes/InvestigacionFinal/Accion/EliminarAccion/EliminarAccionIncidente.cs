using GestionAmbiental.Application.Repositories.Incidentes.InvestigacionFinal;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.Incidentes.InvestigacionFinal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.Accion.EliminarAccion
{
    public class EliminarAccionIncidente : IEliminarAccionIncidenteInvestigacion
    {
        private readonly IEditarInvestigacionFinalReadOnlyRepository _editarReportePreliminarReadOnlyRepository;

        public EliminarAccionIncidente(IEditarInvestigacionFinalReadOnlyRepository editarReportePreliminarReadOnlyRepository)
        {
            _editarReportePreliminarReadOnlyRepository = editarReportePreliminarReadOnlyRepository ?? throw new ArgumentNullException(nameof(editarReportePreliminarReadOnlyRepository));
        }

        public async Task Execute(int IdAccion)
        {
            var accion = new Accion_Incidente_Investigacion();
            accion.Id_Accion_Incidente_Investigacion = IdAccion;
            accion.Estado = EstadosIncidentes.Eliminado.ToString();

            await _editarReportePreliminarReadOnlyRepository.EditarAccionCorrectivaInvestigacion(accion);
        }
    }
}
