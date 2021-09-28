using GestionAmbiental.Domain.DashboardGeneral.Inspecciones;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.DashboardGeneral
{
    public interface IGetDataInspeccionesDashboardRepository
    {
        Task<InspeccionesMAPSSOOutput> InspeccionesActoCondicionBP();
        Task<InspeccionesMAPSSOOutput> InspeccionesEstadosCondicion();
        Task<InspeccionesMAPSSOOutput> TopActosSubestandar(int topLimit);
        Task<InspeccionesMAPSSOOutput> TopCondicionesSubestandar(int topLimit);
        Task<InspeccionesMAPSSOOutput> RankingGerenciaReportante(int topLimit);
        Task<InspeccionesMAPSSOOutput> RankingGerenciaObsVencidas(int topLimit);
        Task<InspeccionesMAPSSOOutput> RankingUsuariosIngresoInspecciones(int topLimit);
        Task<InspeccionesMAPSSOOutput> RankingUsuariosObsVencidas(int topLimit);
    }
}
