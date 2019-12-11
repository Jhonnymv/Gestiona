using GestionAmbiental.Application.Repositories.Reportes;
using GestionAmbiental.Domain.Reportes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Reportes.Inspecciones.GetObservacionesMAPXTipoCondicion
{
    public class GetObservacionesMAPXTipoCondicion : IGetObservacionesMAPXTipoCondicion
    {
        private readonly IGetReportesReadOnlyRepository _getReportesReadOnlyRepository;

        public GetObservacionesMAPXTipoCondicion(IGetReportesReadOnlyRepository getReportesReadOnlyRepository)
        {
            _getReportesReadOnlyRepository = getReportesReadOnlyRepository ?? throw new ArgumentNullException(nameof(getReportesReadOnlyRepository));
        }

        public async Task<List<List<Dimension>>> Execute()
        {
            var output = await _getReportesReadOnlyRepository.GetObservacionesMAPXTipoCondicion();
            return output;
        }
    }
}
