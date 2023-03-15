using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.Repositories.CompromisosMCB.CompromisoCMCB;
using GestionAmbiental.Application.Repositories.CompromisosMCB.EtapaCMCB;

namespace GestionAmbiental.Application.UseCases.CompromisosMCB.EtapaCMCB.NuevoRegistro
{
    public class NuevoRegistro : INuevoRegistro
    {
        private readonly IEtapaReadOnlyRepository _etapaReadOnlyRepository;
        private readonly IGetItemsReadOnlyRepository _getItemsReadOnlyRepository;

        public NuevoRegistro(IEtapaReadOnlyRepository etapaReadOnlyRepository, IGetItemsReadOnlyRepository getItemsReadOnlyRepository)
        {
            _etapaReadOnlyRepository = etapaReadOnlyRepository;
            _getItemsReadOnlyRepository = getItemsReadOnlyRepository;
        }

        public async Task<int> Execute(string Etapa)
        {
            var existentes = await _getItemsReadOnlyRepository.GetEtapas();

            var existe = existentes.Find(x => x.Etapa.Equals(Etapa));

            if (existe != null)
            {
                return existe.Id;
            }
            else
            {
                var input = new Domain.CompromisosMCB.EtapaCMCB
                {
                    Etapa = Etapa
                };
                return await _etapaReadOnlyRepository.RegistrarEtapa(input);
            }
        }
    }
}
