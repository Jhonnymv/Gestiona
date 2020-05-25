using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminarMA.GetReportesPreliminares
{
    public interface IGetReportesPreliminaresMA
    {
        Task<ReportesPreliminaresOutput> Execute(string fecha, int id_Asesor, int id_Usuario, int id_Gerencia, int pagina, int rpp);
    }
}
