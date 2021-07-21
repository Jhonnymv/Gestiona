using GestionAmbiental.Application.Repositories.Incidentes.ReportePreliminar;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.GetReportePreliminar
{
    public class GetReportePreliminar : IGetReportePreliminar
    {
        private readonly IGetReportePreliminarReadOnlyRepository _getReportePreliminarReadOnlyRepository;

        public GetReportePreliminar(IGetReportePreliminarReadOnlyRepository getReportePreliminarReadOnlyRepository)
        {
            _getReportePreliminarReadOnlyRepository = getReportePreliminarReadOnlyRepository ?? throw new ArgumentNullException(nameof(getReportePreliminarReadOnlyRepository));
        }

        public async Task<ReportePreliminarOutput> Execute(int IdReporte)
        {
            var reporte = await _getReportePreliminarReadOnlyRepository.GetReporte(IdReporte);
            var output = new ReportePreliminarOutput(reporte);
            output.Investigacion = await _getReportePreliminarReadOnlyRepository.GetInvestigacionReporte(IdReporte);
            return output;
        }
    }
}
