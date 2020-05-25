using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.GetEstadoReportePreliminar
{
    public interface IGetEstadoReportePreliminar
    {
        Task<string> Execute(int IdReporte);
    }
}
