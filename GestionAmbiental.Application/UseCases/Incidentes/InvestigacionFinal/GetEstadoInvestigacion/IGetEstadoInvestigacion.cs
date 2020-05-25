using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.GetEstadoInvestigacion
{
    public interface IGetEstadoInvestigacion
    {
        Task<string> Execute(int IdInvestigacion);
    }
}
