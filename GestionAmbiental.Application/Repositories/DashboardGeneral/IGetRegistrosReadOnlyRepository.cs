using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.DashboardGeneral
{
    public interface IGetRegistrosReadOnlyRepository
    {
        Task<List<Domain.Inspecciones.InspeccionDetalle>> Observaciones(DateTime startTime, DateTime endTime);
        Task<List<Domain.RAC.RAC>> RACs(DateTime startTime, DateTime endTime);
        Task<List<Domain.Incidentes.ReportePreliminar.Accion_Incidente_Reporte>> AccionesReportePreliminar(DateTime startTime, DateTime endTime);
        Task<List<Domain.Incidentes.ReportePreliminar.Causa_Incidente_Reporte>> CausasReportePreliminar(DateTime startTime, DateTime endTime);
        Task<List<Domain.Incidentes.InvestigacionFinal.Accion_Incidente_Investigacion>> AccionesInvestigacion(DateTime startTime, DateTime endTime);
        Task<List<Domain.Incidentes.InvestigacionFinal.Causa_Incidente_Investigacion>> CausasInvestigacion(DateTime startTime, DateTime endTime);
        Task<List<Domain.Incidentes.ReportePreliminar.Reporte_Preliminar_Incidente>> ProductosDerramadosRP(DateTime startTime, DateTime endTime);
        Task<List<Domain.Incidentes.InvestigacionFinal.Producto_Derramado_Incidente>> ProductosDerramadosInvestigacion(DateTime startTime, DateTime endTime);
    }
}
