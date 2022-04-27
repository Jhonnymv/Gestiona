using GestionAmbiental.Domain.DashboardGeneral.RAC;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.DashboardGeneral.RAC.RACEstadosCondicion
{
    public interface IRACEstadosCondicion
    {
        Task<RACMAPSSOOutput> Execute(int IdGerenciaInsp, int IdEmpresaInsp, string FInicioInsp, string FFinInsp, int IdRespLevInsp, int IdAsesorInsp, string EstadoInsp, int IdGerenciaInspeccionadaInsp);
    }
}
