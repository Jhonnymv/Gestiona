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
        Task<int> RegistrarReportePreliminarDesdeFlashReport(Reporte_Preliminar_Incidente reporte, int Id_Flash_Report);
		Task<int> RegistrarReportePreliminarParte1(Reporte_Preliminar_Incidente reporte);
        Task<int> RegistrarInvolucradoReportePreliminar(Involucrado_Incidente_Reporte involucrado);
        Task<int> RegistrarAccionCorrectivaReportePreliminar(Accion_Incidente_Reporte accion);
        Task<int> RegistrarCausaIncidenteReporte(Causa_Incidente_Reporte causa);
        Task<int> RegistrarReportePreliminarParte2(Reporte_Preliminar_Incidente reporte);
        Task<string> GetUltimoCodigo();
    }
}
