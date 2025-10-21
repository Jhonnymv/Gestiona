using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Reportes.Incidentes
{
    public interface IGetInformacionReportesIncidentes
    {
        Task<ReportesIncidentesOutput> GetNReportesFecha(int tipoIncidente, int nivelIncidente, int gerencia, int supervisor, string FechaInicio, string FechaFin, string frecuencia);
        Task<ReportesIncidentesOutput> GetNReportesTipoIncidente(int tipoIncidente, int nivelIncidente, int gerencia, int supervisor, string FechaInicio, string FechaFin);
        Task<ReportesIncidentesOutput> GetNReportesNivelIncidente(int tipoIncidente, int nivelIncidente, int gerencia, int supervisor, string FechaInicio, string FechaFin);
        Task<ReportesIncidentesOutput> GetNReportesGerenciaIncidente(int tipoIncidente, int nivelIncidente, int gerencia, int supervisor, string FechaInicio, string FechaFin);
        Task<ReportesIncidentesOutput> GetNReportesCausaInmediataIncidente(int tipoIncidente, int nivelIncidente, int gerencia, int supervisor, string FechaInicio, string FechaFin);
        Task<ReportesIncidentesOutput> GetNReportesCausaBasicaIncidente(int tipoIncidente, int nivelIncidente, int gerencia, int supervisor, string FechaInicio, string FechaFin);
    }
}
