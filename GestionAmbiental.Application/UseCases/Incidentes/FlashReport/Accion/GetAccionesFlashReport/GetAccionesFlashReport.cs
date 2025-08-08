using GestionAmbiental.Application.Repositories.Incidentes.FlashReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.FlashReport.Accion.GetAccionesFlashReport
{
    internal class GetAccionesFlashReport : IGetAccionesFlashReport
    {
        private readonly IAccionFlashReportReadOnlyRepository _accionFlashReportReadOnlyRepository;

        public GetAccionesFlashReport(IAccionFlashReportReadOnlyRepository accionFlashReportReadOnlyRepository)
        {
            _accionFlashReportReadOnlyRepository = accionFlashReportReadOnlyRepository ?? throw new ArgumentNullException(nameof(accionFlashReportReadOnlyRepository));
        }

        public async Task<AccionesFlashReportOutput> Execute(int Id_Flash_Report)
        {
            var consulta = await _accionFlashReportReadOnlyRepository.ListarAccionesFlash(Id_Flash_Report);
            var output = new AccionesFlashReportOutput();
            output.AccionesFlashReport = new List<AccionFlashReportOutput>();
            foreach (var item in consulta)
            {
                output.AccionesFlashReport.Add(new AccionFlashReportOutput(item));
            }
            output.NAccionesFlashReports = output.AccionesFlashReport.Count;
            return output;
        }
    }
}
