using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.DashboardGeneral.ObtenerRegistros.Incidentes.InvestigacionFinal.Causas
{
    public interface IObtenerCausasInvestigacion
    {
        Task<List<Domain.Incidentes.InvestigacionFinal.Causa_Incidente_Investigacion>> Execute(string startTime, string endTime);
    }
}
