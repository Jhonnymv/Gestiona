using GestionAmbiental.Domain.Incidentes.ReportePreliminar;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace GestionAmbiental.Application.Repositories.Incidentes.ReportePreliminar
{
    public interface IGetItemsReportePreliminarReadOnlyRepository
    {
        Task<List<Involucrado_Incidente_Reporte>> GetInvolucradosReporte(int IdReportePreliminar);
        Task<List<Accion_Incidente_Reporte>> GetAccionesReporte(int IdReportePreliminar);
        Task<List<Causa_Incidente_Reporte>> GetCausasReporte(int IdReportePreliminar);
        Task<Accion_Incidente_Reporte> GetAccionReporte(int IdAccionCorrectiva);
        Task<Causa_Incidente_Reporte> GetCausaReporte(int IdCausaIncidente);
        Task<Involucrado_Incidente_Reporte> GetInvolucradoReporte(int IdInvolucradoIncidente);
    }
}
