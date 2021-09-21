using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.GetEvidenciasNombre
{
    public interface IGetEvidenciasNombre
    {
        Task<string> Execute(int IdInvestigacion);
    }
}
