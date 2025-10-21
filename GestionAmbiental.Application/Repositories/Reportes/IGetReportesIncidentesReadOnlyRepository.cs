using GestionAmbiental.Application.UseCases.Reportes.Incidentes;
using GestionAmbiental.Domain.Reportes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.Reportes
{
    public interface IGetReportesIncidentesReadOnlyRepository
    {
        Task<ReporteIncidentes> GetNReportes(int tipoIncidente, int nivelIncidente, int gerencia, int supervisor, DateTime FechaInicio, DateTime FechaFin, string frecuencia);
        Task<ReporteIncidentes> GetNReportesTipoIncidente(int tipoIncidente, int nivelIncidente, int gerencia, int supervisor, DateTime FechaInicio, DateTime FechaFin);
        Task<ReporteIncidentes> GetNReportesNivelIncidente(int tipoIncidente, int nivelIncidente, int gerencia, int supervisor, DateTime FechaInicio, DateTime FechaFin);
        Task<ReporteIncidentes> GetNReportesCausasInmediatasIncidente(int tipoIncidente, int nivelIncidente, int gerencia, int supervisor, DateTime FechaInicio, DateTime FechaFin);
        Task<ReporteIncidentes> GetNReportesCausasBasicasIncidente(int tipoIncidente, int nivelIncidente, int gerencia, int supervisor, DateTime FechaInicio, DateTime FechaFin);
        Task<ReporteIncidentes> GetNReportesGerencia(int tipoIncidente, int nivelIncidente, int gerencia, int supervisor, DateTime FechaInicio, DateTime FechaFin);
    }
}
