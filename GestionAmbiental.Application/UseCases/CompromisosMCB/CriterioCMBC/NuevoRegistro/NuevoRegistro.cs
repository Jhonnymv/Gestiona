using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.Repositories.CompromisosMCB.CompromisoCMCB;
using GestionAmbiental.Application.Repositories.CompromisosMCB.CriterioCMCB;

namespace GestionAmbiental.Application.UseCases.CompromisosMCB.CriterioCMBC.NuevoRegistro
{
    public class NuevoRegistro : INuevoRegistro
    {
        private readonly ICriterioReadOnlyRepository _criterioReadOnlyRepository;
        private readonly IGetItemsReadOnlyRepository _getItemsReadOnlyRepository;

        public NuevoRegistro(ICriterioReadOnlyRepository criterioReadOnlyRepository, IGetItemsReadOnlyRepository getItemsReadOnlyRepository)
        {
            _criterioReadOnlyRepository = criterioReadOnlyRepository;
            _getItemsReadOnlyRepository = getItemsReadOnlyRepository;
        }

        public async Task<int> Execute(string Criterio, string Detalle)
        {
            var existentes = await _getItemsReadOnlyRepository.GetCriterios();

            var existe = existentes.Find(x => x.Criterio.Equals(Criterio));

            if (existe != null)
            {
                return existe.Id;
            }
            else
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
}
