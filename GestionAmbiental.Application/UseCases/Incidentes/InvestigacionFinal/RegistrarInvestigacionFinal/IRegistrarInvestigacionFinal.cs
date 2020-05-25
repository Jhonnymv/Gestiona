using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.RegistrarInvestigacionFinal
{
    public interface IRegistrarInvestigacionFinal
    {
        Task<int> Execute(int IdReporte);
    }
}
