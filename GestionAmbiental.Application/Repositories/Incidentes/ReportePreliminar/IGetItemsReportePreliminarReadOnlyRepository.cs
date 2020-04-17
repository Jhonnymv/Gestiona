using GestionAmbiental.Domain.Incidentes.InvestigacionFinal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.Incidentes.ReportePreliminar
{
    public interface IGetItemsReportePreliminarReadOnlyRepository
    {
        Task<List<Involucrado_Incidente>> GetInvolucradosReporte(int IdReportePreliminar);
        Task<List<Accion_Correctiva_Incidente>> GetAccionesReporte(int IdReportePreliminar);
        Task<List<Causa_Incidente>> GetCausasReporte(int IdReportePreliminar);
    }
}
