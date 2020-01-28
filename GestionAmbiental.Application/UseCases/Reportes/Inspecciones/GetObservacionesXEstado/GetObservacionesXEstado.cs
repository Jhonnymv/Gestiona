using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.Repositories.Reportes;
using GestionAmbiental.Domain.Reportes;

namespace GestionAmbiental.Application.UseCases.Reportes.Inspecciones.GetObservacionesXEstado
{
    public class GetObservacionesXEstado : IGetObservacionesXEstado
    {
        private readonly IGetReportesInspeccionesReadOnlyRepository _getReportesReadOnlyRepository;

        public GetObservacionesXEstado(IGetReportesInspeccionesReadOnlyRepository getReportesReadOnlyRepository)
        {
            _getReportesReadOnlyRepository = getReportesReadOnlyRepository ?? throw new ArgumentNullException(nameof(getReportesReadOnlyRepository));
        }

        public async Task<List<Dimension>> Execute()
        {
            var output = await _getReportesReadOnlyRepository.GetObservacionesXEstado();
            output.RemoveAt(1);
            return output;
        }
    }
}
