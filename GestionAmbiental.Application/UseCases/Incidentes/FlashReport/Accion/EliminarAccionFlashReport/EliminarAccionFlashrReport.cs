using GestionAmbiental.Application.Repositories.Incidentes.FlashReport;
using GestionAmbiental.Application.Repositories.Incidentes.ReportePreliminar;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.Incidentes.FlashReport;
using GestionAmbiental.Domain.Incidentes.ReportePreliminar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.FlashReport.Accion.EliminarAccionFlashReport
{
	internal class EliminarAccionFlashrReport : IEliminarAccionFlashReport
	{

		private readonly IAccionFlashReportReadOnlyRepository _accionFlashReportReadOnlyRepository;

		public EliminarAccionFlashrReport(IAccionFlashReportReadOnlyRepository accionFlashReportReadOnlyRepository)
		{
			_accionFlashReportReadOnlyRepository = accionFlashReportReadOnlyRepository ?? throw new ArgumentNullException(nameof(accionFlashReportReadOnlyRepository));
		}

		public async Task Execute(int IdAccion)
		{
			var accion = new Accion_Flash_Report();
			accion.Id_Accion_Flash_Report = IdAccion;
			accion.Estado = EstadosIncidentes.Eliminado.ToString();
			await _accionFlashReportReadOnlyRepository.EditarAccionFlashReport(accion);
		}
	}
}
