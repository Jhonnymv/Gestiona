using GestionAmbiental.Application.Repositories.Incidentes.ReportePreliminar;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.GetEstadoReportePreliminar
{
    public class GetEstadoReportePreliminar : IGetEstadoReportePreliminar
    {
        private readonly IGetReportePreliminarReadOnlyRepository _getReportePreliminarReadOnlyRepository;

        public GetEstadoReportePreliminar(IGetReportePreliminarReadOnlyRepository getReportePreliminarReadOnlyRepository)
        {
            _getReportePreliminarReadOnlyRepository = getReportePreliminarReadOnlyRepository ?? throw new ArgumentNullException(nameof(getReportePreliminarReadOnlyRepository));
        }

        public async Task<string> Execute(int IdReporte)
        {
            return await _getReportePreliminarReadOnlyRepository.GetEstadoReporte(IdReporte);
        }
    }
}
