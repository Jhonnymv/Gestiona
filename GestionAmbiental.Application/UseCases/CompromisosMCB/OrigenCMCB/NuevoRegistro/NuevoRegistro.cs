using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.Repositories.CompromisosMCB.OrigenCMCB;

namespace GestionAmbiental.Application.UseCases.CompromisosMCB.OrigenCMCB.NuevoRegistro
{
    public class NuevoRegistro:INuevoRegistro
    {
        private readonly IOrigenReadOnlyRepository _origenReadOnlyRepository;

        public NuevoRegistro(IOrigenReadOnlyRepository origenReadOnlyRepository)
        {
            _origenReadOnlyRepository = origenReadOnlyRepository ?? throw new ArgumentNullException(nameof(origenReadOnlyRepository));
        }
        public async Task<int> Execute(string Origen)
        {
            var input = new Domain.CompromisosMCB.OrigenCMCB
            {
               Origen = Origen
                
            };
            return await _origenReadOnlyRepository.RegistrarOrigen(input);
        }
    }
}
