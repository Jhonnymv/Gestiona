using GestionAmbiental.Application.Repositories.Reportes;
using GestionAmbiental.Domain.Reportes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Reportes.Inspecciones.GetObservacionesSSOXTipoCondicion
{
    public class GetObservacionesSSOXTipoCondicion : IGetObservacionesSSOXTipoCondicion
    {
        private readonly IGetReportesReadOnlyRepository _getReportesReadOnlyRepository;

        public GetObservacionesSSOXTipoCondicion(IGetReportesReadOnlyRepository getReportesReadOnlyRepository)
        {
            _getReportesReadOnlyRepository = getReportesReadOnlyRepository ?? throw new ArgumentNullException(nameof(getReportesReadOnlyRepository));
        }

        public async Task<List<List<Dimension>>> Execute()
        {
            var output = await _getReportesReadOnlyRepository.GetObservacionesSSOXTipoCondicion();
            return output;
        }
    }
}
