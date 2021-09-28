using GestionAmbiental.Application.Repositories.DashboardGeneral;
using GestionAmbiental.Domain.DashboardGeneral.Inspecciones;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.DashboardGeneral.Inspecciones.InspeccionesActoCondicionBP
{
    public class InspeccionesActoCondicionBP : IInspeccionesActoCondicionBP
    {
        private readonly IGetDataInspeccionesDashboardRepository _getDataDashboardRepository;

        public InspeccionesActoCondicionBP(IGetDataInspeccionesDashboardRepository getDataDashboardRepository)
        {
            _getDataDashboardRepository = getDataDashboardRepository ?? throw new ArgumentNullException(nameof(getDataDashboardRepository));
        }

        public async Task<InspeccionesMAPSSOOutput> Execute()
        {
            return await _getDataDashboardRepository.InspeccionesActoCondicionBP();
        }
    }
}
