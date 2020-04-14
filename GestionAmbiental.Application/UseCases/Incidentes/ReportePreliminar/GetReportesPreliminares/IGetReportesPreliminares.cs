using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.GetReportesPreliminares
{
    public interface IGetReportesPreliminares
    {
        Task<ReportesPreliminaresOutput> Execute(string fecha, int id_Usuario, int id_Gerencia, int pagina, int rpp);
    }
}
