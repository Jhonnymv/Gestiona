using GestionAmbiental.Application.Repositories.Incidentes.InvestigacionFinal;
using GestionAmbiental.Application.Repositories.Incidentes.ReportePreliminar;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.Incidentes.InvestigacionFinal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.Causa.EliminarCausa
{
    public class EliminarCausaIncidenteInvestigacion : IEliminarCausaIncidenteInvestigacion
    {
        private readonly IEditarInvestigacionFinalReadOnlyRepository _editarReportePreliminarReadOnlyRepository;

        public EliminarCausaIncidenteInvestigacion(IEditarInvestigacionFinalReadOnlyRepository editarReportePreliminarReadOnlyRepository)
        {
            _editarReportePreliminarReadOnlyRepository = editarReportePreliminarReadOnlyRepository ?? throw new ArgumentNullException(nameof(editarReportePreliminarReadOnlyRepository));
        }

        public async Task Execute(int Id_Causa_Incidente)
        {
            var input = new Causa_Incidente_Investigacion();
            input.Id_Causa_Incidente_Investigacion = Id_Causa_Incidente;
            input.Estado = EstadosIncidentes.Eliminado.ToString();
            await _editarReportePreliminarReadOnlyRepository.EditarCausaIncidenteInvestigacion(input);
        }
    }
}
