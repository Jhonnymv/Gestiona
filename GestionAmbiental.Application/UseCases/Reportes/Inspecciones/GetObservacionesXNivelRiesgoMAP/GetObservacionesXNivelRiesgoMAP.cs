using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.Repositories.Reportes;
using GestionAmbiental.Domain.Reportes;

namespace GestionAmbiental.Application.UseCases.Reportes.Inspecciones.GetObservacionesXNivelRiesgoMAP
{
    public class GetObservacionesXNivelRiesgoMAP : IGetObservacionesXNivelRiesgoMAP
    {
        private readonly IGetReportesInspeccionesReadOnlyRepository _getReportesReadOnlyRepository;

        public GetObservacionesXNivelRiesgoMAP(IGetReportesInspeccionesReadOnlyRepository getReportesReadOnlyRepository)
        {
            _getReportesReadOnlyRepository = getReportesReadOnlyRepository ?? throw new ArgumentNullException(nameof(getReportesReadOnlyRepository));
        }

        public async Task<List<Dimension>> Execute()
        {
            var output = await _getReportesReadOnlyRepository.GetObservacionesNivelRiesgoMAP();
            output = output.OrderBy(x => x.Index).ToList();
            return output;
        }
    }
}
