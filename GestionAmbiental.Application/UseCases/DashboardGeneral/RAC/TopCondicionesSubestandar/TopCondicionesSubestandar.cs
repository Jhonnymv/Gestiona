using GestionAmbiental.Application.Repositories.DashboardGeneral;
using GestionAmbiental.Domain.DashboardGeneral.RAC;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.DashboardGeneral.RAC.TopCondicionesSubestandar
{
    public class TopCondicionesSubestandar : ITopCondicionesSubestandar
    {
        private readonly IGetDataRACDashboardRepository _getDataDashboardRepository;

        public TopCondicionesSubestandar(IGetDataRACDashboardRepository getDataDashboardRepository)
        {
            _getDataDashboardRepository = getDataDashboardRepository ?? throw new ArgumentNullException(nameof(getDataDashboardRepository));
        }
        public async Task<RACMAPSSOOutput> Execute(int top, int IdGerenciaInsp, int IdEmpresaInsp, string FInicioInsp, string FFinInsp, int IdRespLevInsp, int IdAsesorInsp, string EstadoInsp, int IdGerenciaInspeccionadaInsp)
        {
            //Dar formato a fechas
            DateTime fInicio, fFinal;
            if (FInicioInsp != null)
                fInicio = Convert.ToDateTime(FInicioInsp);
            else
                fInicio = DateTime.MinValue;

            if (FFinInsp != null)
                fFinal = Convert.ToDateTime(FFinInsp + " 23:59:59");
            else
                fFinal = DateTime.MaxValue;
            return await _getDataDashboardRepository.TopCondicionesSubestandar(top, IdGerenciaInsp, IdEmpresaInsp, fInicio, fFinal, IdRespLevInsp, IdAsesorInsp, EstadoInsp, IdGerenciaInspeccionadaInsp);
        }
    }
}
