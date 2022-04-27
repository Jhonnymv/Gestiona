using GestionAmbiental.Domain.DashboardGeneral.RAC;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.DashboardGeneral
{
    public interface IGetDataRACDashboardRepository
    {
        Task<RACMAPSSOOutput> RACActoCondicionBP(int IdGerenciaInsp, int IdEmpresaInsp, DateTime FInicioInsp, DateTime FFinInsp, int IdRespLevInsp, int IdAsesorInsp, string EstadoInsp, int IdGerenciaInspeccionadaInsp);
        Task<RACMAPSSOOutput> RACEstadosCondicion(int IdGerenciaInsp, int IdEmpresaInsp, DateTime FInicioInsp, DateTime FFinInsp, int IdRespLevInsp, int IdAsesorInsp, string EstadoInsp, int IdGerenciaInspeccionadaInsp);
        Task<RACMAPSSOOutput> TopActosSubestandar(int topLimit, int IdGerenciaInsp, int IdEmpresaInsp, DateTime FInicioInsp, DateTime FFinInsp, int IdRespLevInsp, int IdAsesorInsp, string EstadoInsp, int IdGerenciaInspeccionadaInsp);
        Task<RACMAPSSOOutput> TopCondicionesSubestandar(int topLimit, int IdGerenciaInsp, int IdEmpresaInsp, DateTime FInicioInsp, DateTime FFinInsp, int IdRespLevInsp, int IdAsesorInsp, string EstadoInsp, int IdGerenciaInspeccionadaInsp);
        Task<RACMAPSSOOutput> RankingGerenciaReportante(int topLimit, int IdGerenciaInsp, int IdEmpresaInsp, DateTime FInicioInsp, DateTime FFinInsp, int IdRespLevInsp, int IdAsesorInsp, string EstadoInsp, int IdGerenciaInspeccionadaInsp);
        Task<RACMAPSSOOutput> RankingGerenciaObsVencidas(int topLimit, int IdGerenciaInsp, int IdEmpresaInsp, DateTime FInicioInsp, DateTime FFinInsp, int IdRespLevInsp, int IdAsesorInsp, string EstadoInsp, int IdGerenciaInspeccionadaInsp);
        Task<RACMAPSSOOutput> RankingUsuariosIngresoRAC(int topLimit, int IdGerenciaInsp, int IdEmpresaInsp, DateTime FInicioInsp, DateTime FFinInsp, int IdRespLevInsp, int IdAsesorInsp, string EstadoInsp, int IdGerenciaInspeccionadaInsp);
        Task<RACMAPSSOOutput> RankingUsuariosObsVencidas(int topLimit, int IdGerenciaInsp, int IdEmpresaInsp, DateTime FInicioInsp, DateTime FFinInsp, int IdRespLevInsp, int IdAsesorInsp, string EstadoInsp, int IdGerenciaInspeccionadaInsp);
    }
}
