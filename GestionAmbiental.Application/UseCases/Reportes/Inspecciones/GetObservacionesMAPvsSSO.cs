using GestionAmbiental.Application.Repositories.Reportes;
using GestionAmbiental.Domain.Reportes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Reportes.Inspecciones
{
    public class GetObservacionesMAPvsSSO : IGetObservacionesMAPvsSSO
    {
        private readonly IGetReportesReadOnlyRepository _getReportesReadOnlyRepository;

        public GetObservacionesMAPvsSSO(IGetReportesReadOnlyRepository getReportesReadOnlyRepository)
        {
            _getReportesReadOnlyRepository = getReportesReadOnlyRepository ?? throw new ArgumentNullException(nameof(getReportesReadOnlyRepository));
        }

        public async Task<List<Dimension>> Execute()
        {
            var MAP = await _getReportesReadOnlyRepository.GetObservacionesMAP();
            var SSO = await _getReportesReadOnlyRepository.GetObservacionesSSO();

            var output = new List<Dimension>();
            output.Add(MAP);
            output.Add(SSO);

            return output;
        }
    }
}
