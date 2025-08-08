using GestionAmbiental.Application.Repositories.Incidentes.FlashReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.FlashReport.Accion.GetAccionFlashReport
{
    internal class GetAccionesFlashReport : IGetAccionFlashReport
    {

        private readonly IAccionFlashReportReadOnlyRepository _accionFlashReportReadOnlyRepository;

        public GetAccionesFlashReport(IAccionFlashReportReadOnlyRepository accionFlashReportReadOnlyRepository)
        {
            this._accionFlashReportReadOnlyRepository = accionFlashReportReadOnlyRepository ?? throw new ArgumentNullException(nameof(accionFlashReportReadOnlyRepository));
        }

        public async Task<AccionFlashReportOutput> Execute(int Id_Accion_Flash_Report)
        {
            var consulta = await _accionFlashReportReadOnlyRepository.DetalleAccionFlashReport(Id_Accion_Flash_Report);
            var output = new AccionFlashReportOutput(consulta);
            return output;
        }
    }
}
