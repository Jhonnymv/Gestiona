using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminarMA.GetReportesPreliminares
{
    public interface IGetReportesPreliminaresMA
    {
        Task<ReportesPreliminaresOutput> Execute(string fecha_inicio, string fecha_fin, int[] gerencia, int titular, string[] estado, int[] responsable, int[] asesor, string codigo, int pagina, int rpp);
    }
}
