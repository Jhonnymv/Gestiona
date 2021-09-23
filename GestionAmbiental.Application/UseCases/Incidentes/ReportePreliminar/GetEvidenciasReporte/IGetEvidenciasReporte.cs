using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.GetEvidenciasReporte
{
    public interface IGetEvidenciasReporte
    {
        Task<string> Execute(int IdReporte);
    }
}
