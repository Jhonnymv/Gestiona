using GestionAmbiental.Domain.DashboardGeneral.Inspecciones;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.DashboardGeneral
{
    public interface IGetDataInspeccionesDashboardRepository
    {
        Task<InspeccionesMAPSSOOutput> InspeccionesActoCondicionBP(int IdGerenciaInsp, int IdEmpresaInsp, DateTime FInicioInsp, DateTime FFinInsp, int IdRespLevInsp, int IdAsesorInsp, string EstadoInsp, int IdGerenciaInspeccionadaInsp);
        Task<InspeccionesMAPSSOOutput> InspeccionesEstadosCondicion(int IdGerenciaInsp, int IdEmpresaInsp, DateTime FInicioInsp, DateTime FFinInsp, int IdRespLevInsp, int IdAsesorInsp, string EstadoInsp, int IdGerenciaInspeccionadaInsp);
        Task<InspeccionesMAPSSOOutput> TopActosSubestandar(int topLimit, int IdGerenciaInsp, int IdEmpresaInsp, DateTime FInicioInsp, DateTime FFinInsp, int IdRespLevInsp, int IdAsesorInsp, string EstadoInsp, int IdGerenciaInspeccionadaInsp);
        Task<InspeccionesMAPSSOOutput> TopCondicionesSubestandar(int topLimit, int IdGerenciaInsp, int IdEmpresaInsp, DateTime FInicioInsp, DateTime FFinInsp, int IdRespLevInsp, int IdAsesorInsp, string EstadoInsp, int IdGerenciaInspeccionadaInsp);
        Task<InspeccionesMAPSSOOutput> RankingGerenciaReportante(int topLimit, int IdGerenciaInsp, int IdEmpresaInsp, DateTime FInicioInsp, DateTime FFinInsp, int IdRespLevInsp, int IdAsesorInsp, string EstadoInsp, int IdGerenciaInspeccionadaInsp);
        Task<InspeccionesMAPSSOOutput> RankingGerenciaObsVencidas(int topLimit, int IdGerenciaInsp, int IdEmpresaInsp, DateTime FInicioInsp, DateTime FFinInsp, int IdRespLevInsp, int IdAsesorInsp, string EstadoInsp, int IdGerenciaInspeccionadaInsp);
        Task<InspeccionesMAPSSOOutput> RankingUsuariosIngresoInspecciones(int topLimit, int IdGerenciaInsp, int IdEmpresaInsp, DateTime FInicioInsp, DateTime FFinInsp, int IdRespLevInsp, int IdAsesorInsp, string EstadoInsp, int IdGerenciaInspeccionadaInsp);
        Task<InspeccionesMAPSSOOutput> RankingUsuariosObsVencidas(int topLimit, int IdGerenciaInsp, int IdEmpresaInsp, DateTime FInicioInsp, DateTime FFinInsp, int IdRespLevInsp, int IdAsesorInsp, string EstadoInsp, int IdGerenciaInspeccionadaInsp);
    }
}
