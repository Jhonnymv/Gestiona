using GestionAmbiental.Application.Repositories.Reportes;
using GestionAmbiental.Domain.Reportes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Reportes.Inspecciones.GetResumenObservacionesXUsuario
{
    public class GetResumenObservacionesXUsuario : IGetResumenObservacionesXUsuario
    {
        private readonly IGetReportesInspeccionesReadOnlyRepository _getReportesReadOnlyRepository;
        public GetResumenObservacionesXUsuario(IGetReportesInspeccionesReadOnlyRepository getReportesReadOnlyRepository)
        {
            _getReportesReadOnlyRepository = getReportesReadOnlyRepository ?? throw new ArgumentNullException(nameof(getReportesReadOnlyRepository));
        }

        public async Task<List<ObservacionesUsuario>> Execute(string filtro)
        {
            var output = await _getReportesReadOnlyRepository.GetResumenObservacionesXUsuario(filtro);
            return output;
        }
    }
}
