using GestionAmbiental.Domain.Incidentes.InvestigacionFinal;
using GestionAmbiental.Domain.Incidentes.ReportePreliminar;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.Incidentes.ReportePreliminar
{
    public interface IGetReportePreliminarReadOnlyRepository
    {
        Task<List<Reporte_Preliminar_Incidente>> GetReportes(DateTime? fechaInicio,DateTime? fechaFin, int[] gerencia, int titular, List<string> estado, int[] responsable, int[] asesor, string codigo);
        Task<List<Reporte_Preliminar_Incidente>> GetReportesMA(DateTime? fechaInicio, DateTime? fechaFin, int[] gerencia, int titular, List<string> estado, int[] responsable, int[] asesor, string codigo);
        Task<Reporte_Preliminar_Incidente> GetReporte(int IdReporte);
        Task<string> GetEstadoReporte(int IdReporte);
        Task<string> GetEvidenciaReporteMA(int IdReporte);
        Task<bool> GetInvestigacionReporte(int IdReporte);
        Task<string> GetEvidenciasReporte(int IdReporte);
    }
}
