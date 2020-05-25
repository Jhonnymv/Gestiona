using GestionAmbiental.Domain.Incidentes.InvestigacionFinal;
using GestionAmbiental.Domain.Incidentes.ReportePreliminar;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.Incidentes.ReportePreliminar
{
    public interface IEditarReportePreliminarReadOnlyRepository
    {
        Task<int> EditarReportePreliminar(Reporte_Preliminar_Incidente reporte);
        Task<int> EditarInvolucradoReportePreliminar(Involucrado_Incidente_Reporte involucrado);
        Task<int> EditarAccionCorrectivaReportePreliminar(Accion_Incidente_Reporte accion);
        Task<int> EditarCausaIncidenteReporte(Causa_Incidente_Reporte causa);
    }
}
