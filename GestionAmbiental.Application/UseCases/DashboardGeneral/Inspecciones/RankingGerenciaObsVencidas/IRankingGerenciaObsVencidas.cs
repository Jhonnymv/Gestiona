using GestionAmbiental.Domain.DashboardGeneral.Inspecciones;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.DashboardGeneral.Inspecciones.RankingGerenciaObsVencidas
{
    public interface IRankingGerenciaObsVencidas
    {
        Task<InspeccionesMAPSSOOutput> Execute(int top);
    }
}
