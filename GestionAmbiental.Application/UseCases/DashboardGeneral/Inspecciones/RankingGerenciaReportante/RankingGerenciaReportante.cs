using GestionAmbiental.Application.Repositories.DashboardGeneral;
using GestionAmbiental.Domain.DashboardGeneral.Inspecciones;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.DashboardGeneral.Inspecciones.RankingGerenciaReportante
{
    public class RankingGerenciaReportante : IRankingGerenciaReportante
    {

        private readonly IGetDataInspeccionesDashboardRepository _getDataDashboardRepository;

        public RankingGerenciaReportante(IGetDataInspeccionesDashboardRepository getDataDashboardRepository)
        {
            _getDataDashboardRepository = getDataDashboardRepository ?? throw new ArgumentNullException(nameof(getDataDashboardRepository));
        }

        public async Task<InspeccionesMAPSSOOutput> Execute(int top)
        {
            return await _getDataDashboardRepository.RankingGerenciaReportante(top);
        }
    }
}
