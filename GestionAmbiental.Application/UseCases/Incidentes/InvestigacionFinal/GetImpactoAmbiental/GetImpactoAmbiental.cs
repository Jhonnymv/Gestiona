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
            var output = new ImpactoAmbientalOutput(impacto);
            return output;
        }
    }
}
