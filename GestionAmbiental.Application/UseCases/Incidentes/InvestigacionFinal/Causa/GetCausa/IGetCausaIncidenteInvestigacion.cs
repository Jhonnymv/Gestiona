using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.Causa.GetCausa
{
    public interface IGetCausaIncidenteInvestigacion
    {
        Task<CausaIncidenteReporteOutput> Execute(int IdCausa);
    }
}
