using GestionAmbiental.Application.Repositories.Reportes;
using GestionAmbiental.Domain.Reportes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Reportes.Inspecciones.GetObservacionesXUsuario
{
    public class GetObservacionesXUsuario : IGetObservacionesXUsuario
    {
        private readonly IGetReportesInspeccionesReadOnlyRepository _getReportesReadOnlyRepository;

        public GetObservacionesXUsuario(IGetReportesInspeccionesReadOnlyRepository getReportesReadOnlyRepository)
        {
            _getReportesReadOnlyRepository = getReportesReadOnlyRepository ?? throw new ArgumentNullException(nameof(getReportesReadOnlyRepository));
        }

        public async Task<List<Dimension>> Execute()
        {
            var data = await _getReportesReadOnlyRepository.GetObservacionesXUsuario();
            return data;
        }
    }
}
