using GestionAmbiental.Application.Repositories.Incidentes.InvestigacionFinal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.GetImpactoAmbiental
{
    public class GetImpactoAmbiental : IGetImpactoAmbiental
    {
        private readonly IGetInvestigacionFinalReadOnlyRepository _getInvestigacionFinalReadOnlyRepository;

        public GetImpactoAmbiental(IGetInvestigacionFinalReadOnlyRepository getInvestigacionFinalReadOnlyRepository)
        {
            _getInvestigacionFinalReadOnlyRepository = getInvestigacionFinalReadOnlyRepository ?? throw new ArgumentNullException(nameof(getInvestigacionFinalReadOnlyRepository));
        }

        public async Task<ImpactoAmbientalOutput> Execute(int IdInvestigacion)
        {
            var impacto = await _getInvestigacionFinalReadOnlyRepository.GetImpactoAmbiental(IdInvestigacion);
            if (impacto == null)
            {
                //throw new KeyNotFoundException($"No se encontró el impacto ambiental para la investigación con ID {IdInvestigacion}");
                return null;
            }
            var output = new ImpactoAmbientalOutput(impacto);
            return output;
        }
    }
}
