using GestionAmbiental.Application.Repositories.Reportes;
using GestionAmbiental.Domain.Reportes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Reportes.Inspecciones.GetObservacionesXActoCondicionSSO
{
    public class GetObservacionesXActoCondicionSSO : IGetObservacionesXActoCondicionSSO
    {
        private readonly IGetReportesReadOnlyRepository _getReportesReadOnlyRepository;

        public GetObservacionesXActoCondicionSSO(IGetReportesReadOnlyRepository getReportesReadOnlyRepository)
        {
            _getReportesReadOnlyRepository = getReportesReadOnlyRepository ?? throw new ArgumentNullException(nameof(getReportesReadOnlyRepository));
        }

        public async Task<List<Dimension>> Execute()
        {
            var output = await _getReportesReadOnlyRepository.GetObservacionesTipoInspeccionSSO();
            output = output.OrderBy(x => x.Index).ToList();
            return output;
        }
    }
}
