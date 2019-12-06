using GestionAmbiental.Application.Repositories.Reportes;
using GestionAmbiental.Domain.Reportes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Reportes.Inspecciones
{
    public class GetInspeccionesDiarias : IGetInspeccionesDiarias
    {
        private readonly IGetReportesReadOnlyRepository _getReportesReadOnlyRepository;

        public GetInspeccionesDiarias(IGetReportesReadOnlyRepository getReportesReadOnlyRepository)
        {
            _getReportesReadOnlyRepository = getReportesReadOnlyRepository ?? throw new ArgumentNullException(nameof(getReportesReadOnlyRepository));
        }

        public async Task<List<Dimension>> Execute()
        {
            return await _getReportesReadOnlyRepository.GetInspeccionesDiarias();
        }
    }
}
