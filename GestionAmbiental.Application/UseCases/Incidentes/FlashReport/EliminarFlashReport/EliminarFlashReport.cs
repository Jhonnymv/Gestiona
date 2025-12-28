using GestionAmbiental.Application.Repositories.Incidentes.FlashReport;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.FlashReport.EliminarFlashReport
{
    public class EliminarFlashReport : IEliminarFlashReport
    {
        private readonly IFlashReportReadOnlyRepository _flashReportReadOnlyRepository;
        public EliminarFlashReport(IFlashReportReadOnlyRepository flashReportReadOnlyRepository)
        {
            _flashReportReadOnlyRepository = flashReportReadOnlyRepository ?? throw new ArgumentNullException(nameof(flashReportReadOnlyRepository));
        }

        public async Task<int> Execute(int IdFlashReport)
        {
            return await _flashReportReadOnlyRepository.EliminarFlashReport(IdFlashReport);
        }
    }
}
