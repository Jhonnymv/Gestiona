using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.Repositories.CompromisosMCB.ComponenteCMCB;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.CompromisosMCB.ComponentesCMCB.NuevoRegistro
{
    public class NuevoRegistro:INuevoRegistro
    {
        private readonly IComponeteReadOnlyRepository _componeteReadOnlyRepository;

        public NuevoRegistro(IComponeteReadOnlyRepository componeteReadOnlyRepository)
        {
            _componeteReadOnlyRepository = componeteReadOnlyRepository ?? throw new ArgumentNullException(nameof(componeteReadOnlyRepository));
        }
        public async Task<int> Execute(string Tipo, string Componente, string Servicio)
        {
            var input = new Domain.CompromisosMCB.ComponenteCMCB
            {
                Tipo = Tipo,
                Componente = Componente,
                Servicio = Servicio
                
            };
            return await _componeteReadOnlyRepository.RegistrarComponente(input);
        }
    }
}
