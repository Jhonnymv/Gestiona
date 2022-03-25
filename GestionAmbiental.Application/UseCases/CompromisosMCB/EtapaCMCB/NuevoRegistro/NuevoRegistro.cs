using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.Repositories.CompromisosMCB.EtapaCMCB;

namespace GestionAmbiental.Application.UseCases.CompromisosMCB.EtapaCMCB.NuevoRegistro
{
    public class NuevoRegistro:INuevoRegistro
    {
        private readonly IEtapaReadOnlyRepository _etapaReadOnlyRepository;

        public NuevoRegistro(IEtapaReadOnlyRepository etapaReadOnlyRepository)
        {
            _etapaReadOnlyRepository = etapaReadOnlyRepository ?? throw new ArgumentNullException(nameof(etapaReadOnlyRepository));
        }
        public async Task<int> Execute(string Etapa)
        {
            var input = new Domain.CompromisosMCB.EtapaCMCB
            {
                Etapa = Etapa
                
            };
            return await _etapaReadOnlyRepository.RegistrarEtapa(input);
        }
    }
}
