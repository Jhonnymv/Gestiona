using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.Repositories.CompromisosMCB.AspectoCMCB;

namespace GestionAmbiental.Application.UseCases.CompromisosMCB.AspectoCMCB.NuevoRegistro
{
    public class NuevoRegistro:INuevoRegistro
    {
        private readonly IAspectoReadOnlyRepository _aspectoReadOnlyRepository;

        public NuevoRegistro(IAspectoReadOnlyRepository aspectoReadOnlyRepository)
        {
            _aspectoReadOnlyRepository = aspectoReadOnlyRepository ?? throw new ArgumentNullException(nameof(aspectoReadOnlyRepository));
        }
        public async Task<int> Execute(string Aspecto)
        {
            var input = new Domain.CompromisosMCB.AspectoCMCB
            {
                Aspecto = Aspecto
            };
            return await _aspectoReadOnlyRepository.RegistrarAspecto(input);
        }
    }
}
