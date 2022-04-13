using GestionAmbiental.Application.Repositories.DashboardGeneral;
using GestionAmbiental.Domain.DashboardGeneral.Inspecciones;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.DashboardGeneral.Inspecciones.TopCondicionesSubestandar
{
    public class TopCondicionesSubestandar : ITopCondicionesSubestandar
    {
        private readonly IGetDataInspeccionesDashboardRepository _getDataDashboardRepository;

        public TopCondicionesSubestandar(IGetDataInspeccionesDashboardRepository getDataDashboardRepository)
        {
            _getDataDashboardRepository = getDataDashboardRepository ?? throw new ArgumentNullException(nameof(getDataDashboardRepository));
        }
        public async Task<InspeccionesMAPSSOOutput> Execute(int top, int IdGerenciaInsp, int IdEmpresaInsp, string FInicioInsp, string FFinInsp, int IdRespLevInsp, int IdAsesorInsp, string EstadoInsp, int IdGerenciaInspeccionadaInsp)
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
