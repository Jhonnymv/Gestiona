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
        Task<List<Reporte_Preliminar_Incidente>> GetReportes(DateTime? fecha,int id_Asesor, int id_Usuario, int id_Gerencia);
        Task<List<Reporte_Preliminar_Incidente>> GetReportesMA(DateTime? fecha,int id_Asesor, int id_Gerencia, int id_Usuario);
        Task<Reporte_Preliminar_Incidente> GetReporte(int IdReporte);
        Task<string> GetEstadoReporte(int IdReporte);
        
    }
}
