using GestionAmbiental.Application.Repositories.DashboardGeneral;
using GestionAmbiental.Domain.DashboardGeneral.Inspecciones;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.DashboardGeneral.Inspecciones.TopActosSubestandar
{
    public class TopActosSubestandar : ITopActosSubestandar
    {
        private readonly IGetDataInspeccionesDashboardRepository _getDataDashboardRepository;

        public TopActosSubestandar(IGetDataInspeccionesDashboardRepository getDataDashboardRepository)
        {
            _getDataDashboardRepository = getDataDashboardRepository ?? throw new ArgumentNullException(nameof(getDataDashboardRepository));
        }
        public async Task<InspeccionesMAPSSOOutput> Execute(int top)
        {
            return await _getDataDashboardRepository.TopActosSubestandar(top);
        }
    }
}
