using GestionAmbiental.Application.Repositories.Incidentes.ReportePreliminar;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.GetEvidenciasReporte
{
    public class GetEvidenciasReporte : IGetEvidenciasReporte
    {
        private readonly IGetReportePreliminarReadOnlyRepository _getReportePreliminarReadOnlyRepository;

        public GetEvidenciasReporte(IGetReportePreliminarReadOnlyRepository getReportePreliminarReadOnlyRepository)
        {
            _getReportePreliminarReadOnlyRepository = getReportePreliminarReadOnlyRepository ?? throw new ArgumentNullException(nameof(getReportePreliminarReadOnlyRepository));
        }
        public async Task<string> Execute(int IdReporte)
        {
            return await _getReportePreliminarReadOnlyRepository.GetEvidenciasReporte(IdReporte);
        }
    }
}
