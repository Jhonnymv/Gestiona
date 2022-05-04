using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.DashboardGeneral.ObtenerRegistros.Incidentes.ReportesPreliminares.Causas
{
    public interface IObtenerCausasRP
    {
        Task<List<Domain.Incidentes.ReportePreliminar.Causa_Incidente_Reporte>> Execute(string startTime, string endTime);
    }
}
