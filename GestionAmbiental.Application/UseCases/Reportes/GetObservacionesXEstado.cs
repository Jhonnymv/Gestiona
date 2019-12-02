using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.Repositories.Reportes;
using GestionAmbiental.Domain.Reportes;

namespace GestionAmbiental.Application.UseCases.Reportes
{
    public class GetObservacionesXEstado : IGetObservacionesXEstado
    {
        private readonly IGetReportesReadOnlyRepository _getReportesReadOnlyRepository;

        public GetObservacionesXEstado(IGetReportesReadOnlyRepository getReportesReadOnlyRepository)
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
