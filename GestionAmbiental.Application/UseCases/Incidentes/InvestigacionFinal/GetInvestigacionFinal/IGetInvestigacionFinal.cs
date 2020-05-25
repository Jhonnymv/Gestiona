using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.GetInvestigacionFinal
{
    public interface IGetInvestigacionFinal
    {
        Task<InvestigacionFinalOutput> Execute(int IdInvestigacion);
    }
}
