using GestionAmbiental.Application.Repositories.Incidentes.InvestigacionFinal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.GetEstadoInvestigacion
{
    public class GetEstadoInvestigacion : IGetEstadoInvestigacion
    {
        private readonly IGetInvestigacionFinalReadOnlyRepository _getInvestigacionFinalReadOnlyRepository;
        public GetEstadoInvestigacion(IGetInvestigacionFinalReadOnlyRepository getInvestigacionFinalReadOnlyRepository)
        {
            _getInvestigacionFinalReadOnlyRepository = getInvestigacionFinalReadOnlyRepository ?? throw new ArgumentNullException(nameof(getInvestigacionFinalReadOnlyRepository));
        }
        public async Task<string> Execute(int IdInvestigacion)
        {
            return await _getInvestigacionFinalReadOnlyRepository.GetEstadoInvestigacion(IdInvestigacion);
        }
    }
}
