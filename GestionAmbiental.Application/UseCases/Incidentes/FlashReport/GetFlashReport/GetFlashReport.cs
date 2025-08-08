using GestionAmbiental.Application.Repositories.Incidentes.FlashReport;
using GestionAmbiental.Application.Repositories.Incidentes.ReportePreliminar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.FlashReport.GetFlashReport
{
    internal class GetFlashReport : IGetFlashReport
    {
        private readonly IFlashReportReadOnlyRepository _flashReportReadOnlyRepository;

        public GetFlashReport(IFlashReportReadOnlyRepository flashReportReadOnlyRepository)
        {
            _flashReportReadOnlyRepository = flashReportReadOnlyRepository ?? throw new ArgumentNullException(nameof(flashReportReadOnlyRepository));
        }

        public async Task<FlashReportOutput> Execute(int Id)
        {
            var reporte = await _flashReportReadOnlyRepository.DetalleFlashReport(Id);
            var output = new FlashReportOutput(reporte);
            return output;
        }
    }
}