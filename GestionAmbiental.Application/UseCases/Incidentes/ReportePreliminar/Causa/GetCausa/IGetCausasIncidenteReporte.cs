using GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.Causa.GetCausa;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.GetCausaIncidenteReporte
{
    public interface IGetCausasIncidenteReporte
    {
        Task<List<CausaIncidenteReporteOutput>> Execute(int IdReporte);
    }
}
