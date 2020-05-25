using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.Causa.GetCausa
{
    public interface IGetCausasIncidenteInvestigacion
    {
        Task<List<CausaIncidenteReporteOutput>> Execute(int IdInvestigacion);
    }
}
