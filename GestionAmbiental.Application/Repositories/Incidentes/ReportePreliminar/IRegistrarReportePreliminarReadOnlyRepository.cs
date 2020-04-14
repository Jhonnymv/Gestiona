using GestionAmbiental.Domain.Incidentes.InvestigacionFinal;
using GestionAmbiental.Domain.Incidentes.ReportePreliminar;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.Incidentes.ReportePreliminar
{
    public interface IRegistrarReportePreliminarReadOnlyRepository
    {
        Task<int> RegistrarReportePreliminarParte1(Reporte_Preliminar_Incidente reporte);
        Task<int> RegistrarInvolucradoReportePreliminar(Involucrado_Incidente involucrado);
        Task<int> RegistrarAccionCorrectivaReportePreliminar(Accion_Correctiva_Incidente accion);
        Task<int> RegistrarReportePreliminarParte2(Reporte_Preliminar_Incidente reporte);
        Task<string> GetUltimoCodigo();
    }
}
