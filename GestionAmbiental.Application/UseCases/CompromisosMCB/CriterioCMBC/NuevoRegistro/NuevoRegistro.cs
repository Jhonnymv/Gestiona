using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.Repositories.CompromisosMCB.CriterioCMCB;

namespace GestionAmbiental.Application.UseCases.CompromisosMCB.CriterioCMBC.NuevoRegistro
{
    public class NuevoRegistro:INuevoRegistro
    {
        private readonly ICriterioReadOnlyRepository _criterioReadOnlyRepository;

        public NuevoRegistro(ICriterioReadOnlyRepository criterioReadOnlyRepository)
        {
            _criterioReadOnlyRepository = criterioReadOnlyRepository ?? throw new ArgumentNullException(nameof(criterioReadOnlyRepository));
        }
        public async Task<int> Execute(string Criterio, string Detalle)
        {
            var input = new Domain.CompromisosMCB.CriterioCMCB
            {
                Criterio = Criterio,
                Detalle = Detalle,
            };
            return await _criterioReadOnlyRepository.RegistrarCriterio(input);
        }
    }
}
