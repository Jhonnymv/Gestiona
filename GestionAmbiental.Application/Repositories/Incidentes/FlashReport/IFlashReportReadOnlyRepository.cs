using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.Incidentes.FlashReport;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.Incidentes.FlashReport
{
    public interface IFlashReportReadOnlyRepository
    {
        Task<List<Flash_Report>> ListarFlashReport(DateTime? fechaInicio, DateTime? fechaFin, int[] gerencia, int[] titular, List<string> estado, int[] responsable, int[] asesor, string codigo);
        Task<Flash_Report> DetalleFlashReport(int id);
        Task<int> EditarFlashReport(Flash_Report flashReport);
        Task<int> EliminarFlashReport(int id);
        Task<int> RegistrarFlashReport(Flash_Report flashReport);
        Task<string> GetUltimoCodigo();
		Task<string> GetEvidenciasReporte(int IdFlashReport);
        Task<bool> EliminarEvidencia(int IdFlashReport, string NombreArchivo);
    }
}
