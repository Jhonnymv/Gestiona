using GestionAmbiental.Domain.Reportes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.Reportes
{
    public interface IGetReportesReportePreliminarIncidenteReadOnlyRepository
    {
        Task<List<Dimension>> GetNReportes(int tipoIncidente, int nivelIncidente, int gerencia, int supervisor, DateTime FechaInicio, DateTime FechaFin,string frecuencia);
        Task<List<Dimension>> GetNReportesTipoIncidente(int tipoIncidente, int nivelIncidente, int gerencia, int supervisor, DateTime FechaInicio, DateTime FechaFin);
        Task<List<Dimension>> GetNReportesNivelIncidente(int tipoIncidente, int nivelIncidente, int gerencia, int supervisor, DateTime FechaInicio, DateTime FechaFin);
        Task<List<Dimension>> GetNReportesCausaIncidente(int tipoIncidente, int nivelIncidente, int gerencia, int supervisor, DateTime FechaInicio, DateTime FechaFin);
        Task<List<Dimension>> GetNReportesGerencia(int tipoIncidente, int nivelIncidente, int gerencia, int supervisor, DateTime FechaInicio, DateTime FechaFin);
    }
}
