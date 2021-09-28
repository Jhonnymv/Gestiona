using GestionAmbiental.Domain.DashboardGeneral.Inspecciones;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.DashboardGeneral.Inspecciones.RankingUsuariosObsVencidas
{
    public interface IRankingUsuariosObsVencidas
    {
        Task<InspeccionesMAPSSOOutput> Execute(int top);
    }
}
