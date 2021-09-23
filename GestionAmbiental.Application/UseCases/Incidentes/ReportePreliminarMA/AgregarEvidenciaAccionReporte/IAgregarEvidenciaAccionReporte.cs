using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminarMA.AgregarEvidenciaAccionReporte
{
    public interface IAgregarEvidenciaAccionReporte
    {
        Task<bool> Execute(int IdReporte, string Evidencias, string EvidenciasNombres);
    }
}
