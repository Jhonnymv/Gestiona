using GestionAmbiental.Application.Repositories.Incidentes.InvestigacionFinal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Notificaciones.Incidentes.NuevaInvestigacion
{
    public class GetInvestigacionFinal : IGetInvestigacionFinal
    {
        private readonly IGetInvestigacionFinalReadOnlyRepository _getInvestigacionFinalReadOnlyRepository;

        public GetInvestigacionFinal(IGetInvestigacionFinalReadOnlyRepository getInvestigacionFinalReadOnlyRepository)
        {
            _getInvestigacionFinalReadOnlyRepository = getInvestigacionFinalReadOnlyRepository ?? throw new ArgumentNullException(nameof(getInvestigacionFinalReadOnlyRepository));
        }

        public async Task<InvestigacionFinalOutput> Execute(int IdInvestigacion)
        {
            var investigacion = await _getInvestigacionFinalReadOnlyRepository.GetInvestigacionFinal(IdInvestigacion);
            var output = new InvestigacionFinalOutput(investigacion);
            return output;
        }
    }
}
