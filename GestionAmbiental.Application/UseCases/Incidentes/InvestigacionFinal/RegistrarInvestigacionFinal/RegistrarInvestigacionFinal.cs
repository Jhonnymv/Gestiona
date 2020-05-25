using GestionAmbiental.Application.Repositories.Incidentes.InvestigacionFinal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.RegistrarInvestigacionFinal
{
    public class RegistrarInvestigacionFinal : IRegistrarInvestigacionFinal
    {
        private readonly IRegistrarInvestigacionFinalReadOnlyRepository _registrarInvestigacionFinalReadOnlyRepository;

        public RegistrarInvestigacionFinal(IRegistrarInvestigacionFinalReadOnlyRepository registrarInvestigacionFinalReadOnlyRepository)
        {
            _registrarInvestigacionFinalReadOnlyRepository = registrarInvestigacionFinalReadOnlyRepository ?? throw new ArgumentNullException(nameof(registrarInvestigacionFinalReadOnlyRepository));
        }

        public async Task<int> Execute(int IdReporte)
        {
            var idInvestigacion = await _registrarInvestigacionFinalReadOnlyRepository.RegistrarDeReportePreliminar(IdReporte);
                        
            await _registrarInvestigacionFinalReadOnlyRepository.RegistrarImpactoAmbiental(idInvestigacion);
            return idInvestigacion;
        }
    }
}
