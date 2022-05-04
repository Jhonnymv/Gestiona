using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.DashboardGeneral.ObtenerRegistros.Incidentes.InvestigacionFinal.Acciones
{
    public interface IObtenerAccionesInvestigacion
    {
        Task<List<Domain.Incidentes.InvestigacionFinal.Accion_Incidente_Investigacion>> Execute(string startTime, string endTime);
    }
}
