using GestionAmbiental.Application.Repositories.Incidentes.ReportePreliminar;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.Incidentes.ReportePreliminar;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.Causa.EliminarCausa
{
    public class EliminarCausaIncidenteReporte : IEliminarCausaIncidenteReporte
    {
        private readonly IEditarReportePreliminarReadOnlyRepository _editarReportePreliminarReadOnlyRepository;

        public EliminarCausaIncidenteReporte(IEditarReportePreliminarReadOnlyRepository editarReportePreliminarReadOnlyRepository)
        {
            _editarReportePreliminarReadOnlyRepository = editarReportePreliminarReadOnlyRepository ?? throw new ArgumentNullException(nameof(editarReportePreliminarReadOnlyRepository));
        }

        public async Task Execute(int Id_Causa_Incidente)
        {
            var input = new Causa_Incidente_Reporte();
            input.Id_Causa_Incidente_Reporte = Id_Causa_Incidente;
            input.Estado = EstadosIncidentes.Eliminado.ToString();
            await _editarReportePreliminarReadOnlyRepository.EditarCausaIncidenteReporte(input);
        }
    }
}
