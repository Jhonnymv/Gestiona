using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.DashboardGeneral.ObtenerRegistros.Incidentes.ReportesPreliminares.Acciones
{
    public interface IObtenerAccionesRP
    {
        Task<List<Domain.Incidentes.ReportePreliminar.Accion_Incidente_Reporte>> Execute(string startTime, string endTime);
    }
}
