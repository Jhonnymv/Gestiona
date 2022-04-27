using GestionAmbiental.Domain.DashboardGeneral.RAC;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.DashboardGeneral.RAC.RankingGerenciaReportante
{
    public interface  IRankingGerenciaReportante
    {
        Task<RACMAPSSOOutput> Execute(int top, int IdGerenciaInsp, int IdEmpresaInsp, string FInicioInsp, string FFinInsp, int IdRespLevInsp, int IdAsesorInsp, string EstadoInsp, int IdGerenciaInspeccionadaInsp);
    }
}
