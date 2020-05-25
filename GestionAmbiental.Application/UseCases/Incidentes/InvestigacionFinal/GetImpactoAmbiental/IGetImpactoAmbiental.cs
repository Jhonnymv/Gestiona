using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.GetImpactoAmbiental
{
    public interface IGetImpactoAmbiental
    {
        Task<ImpactoAmbientalOutput> Execute(int IdInvestigacion);
    }
}
