using GestionAmbiental.Application.Repositories.Incidentes.FlashReport;
using GestionAmbiental.Application.Repositories.Incidentes.ReportePreliminar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.FlashReport.GetEvidenciasFlashReport
{
	internal class GetEvidenciasFlashReport : IGetEvidenciasFlashReport
	{

		private readonly IFlashReportReadOnlyRepository _flashReportReadOnlyRepository;

		public GetEvidenciasFlashReport(IFlashReportReadOnlyRepository flashReportReadOnlyRepository)
		{
			_flashReportReadOnlyRepository = flashReportReadOnlyRepository ?? throw new ArgumentNullException(nameof(flashReportReadOnlyRepository));
		}

		public async Task<string> Execute(int IdFlashReport)
		{
			return await _flashReportReadOnlyRepository.GetEvidenciasReporte(IdFlashReport);
		}
	}
}
