using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminarMA.GetEvidenciaReportePreliminar
{
    public interface IGetEvidenciaReportePreliminar
    {
        Task<string[]> Execute(int IdReporte);
    }
}
