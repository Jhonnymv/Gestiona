using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.Repositories.CompromisosMCB.AutoridadCMCB;

namespace GestionAmbiental.Application.UseCases.CompromisosMCB.AutoridadCMCB.NuevoRegistro
{
    public class NuevoRegistro:INuevoRegistro
    {
        private readonly IAutoridadReadOnlyRepository _autoridadReadOnlyRepository;

        public NuevoRegistro(IAutoridadReadOnlyRepository autoridadReadOnlyRepository)
        {
            _autoridadReadOnlyRepository = autoridadReadOnlyRepository ?? throw new ArgumentNullException(nameof(autoridadReadOnlyRepository));
        }
        public async Task<int> Execute(string Autoridad)
        {
            var input = new Domain.CompromisosMCB.AutoridadCMCB
            {
                Autoridad = Autoridad,
            };
            return await _autoridadReadOnlyRepository.RegistrarAutoridad(input);
        }
    }
}
