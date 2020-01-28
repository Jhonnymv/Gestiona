using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.Repositories.Reportes;
using GestionAmbiental.Application.UseCases.Reportes.Inspecciones.GetObservacionesXActoCondicionMAP;
using GestionAmbiental.Domain.Reportes;

namespace GestionAmbiental.Application.UseCases.Reportes.Inspecciones.GetObservacionesXActoCondicionMAP
{
    public class GetObservacionesXActoCondicionMAP : IGetObservacionesXActoCondicionMAP
    {
        private readonly IGetReportesInspeccionesReadOnlyRepository _getReportesReadOnlyRepository;

        public GetObservacionesXActoCondicionMAP(IGetReportesInspeccionesReadOnlyRepository getReportesReadOnlyRepository)
        {
            _getReportesReadOnlyRepository = getReportesReadOnlyRepository ?? throw new ArgumentNullException(nameof(getReportesReadOnlyRepository));
        }

        public async Task<List<Dimension>> Execute()
        {
            var output = await _getReportesReadOnlyRepository.GetObservacionesTipoInspeccionMAP();
            output = output.OrderBy(x => x.Index).ToList();
            return output;
        }
    }
}
