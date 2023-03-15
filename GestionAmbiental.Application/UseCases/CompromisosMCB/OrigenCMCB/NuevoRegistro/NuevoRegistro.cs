using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.Repositories.CompromisosMCB.CompromisoCMCB;
using GestionAmbiental.Application.Repositories.CompromisosMCB.OrigenCMCB;

namespace GestionAmbiental.Application.UseCases.CompromisosMCB.OrigenCMCB.NuevoRegistro
{
    public class NuevoRegistro : INuevoRegistro
    {
        private readonly IOrigenReadOnlyRepository _origenReadOnlyRepository;
        private readonly IGetItemsReadOnlyRepository _getItemsReadOnlyRepository;

        public NuevoRegistro(IOrigenReadOnlyRepository origenReadOnlyRepository, IGetItemsReadOnlyRepository getItemsReadOnlyRepository)
        {
            _origenReadOnlyRepository = origenReadOnlyRepository;
            _getItemsReadOnlyRepository = getItemsReadOnlyRepository;
        }

        public async Task<int> Execute(string Origen)
        {
            var origenes = await _getItemsReadOnlyRepository.GetOrigenes();

            var origen = origenes.Find(x => x.Origen.Equals(Origen));

            if (origen != null)
            {
                return origen.Id;
            }
            else
            {
                var input = new Domain.CompromisosMCB.OrigenCMCB
                {
                    Origen = Origen

                };
                return await _origenReadOnlyRepository.RegistrarOrigen(input);
            }
        }
    }
}
