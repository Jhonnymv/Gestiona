using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.GetEvidenciaInvestigacion
{
    public interface IGetEvidenciaInvestigacion
    {
        Task<string[]> Execute(int IdInvestigacion, string Tipo);
    }
}
