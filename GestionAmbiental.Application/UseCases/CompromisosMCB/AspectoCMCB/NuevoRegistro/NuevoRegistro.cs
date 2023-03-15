using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.Repositories.CompromisosMCB.AspectoCMCB;
using GestionAmbiental.Application.Repositories.CompromisosMCB.CompromisoCMCB;

namespace GestionAmbiental.Application.UseCases.CompromisosMCB.AspectoCMCB.NuevoRegistro
{
    public class NuevoRegistro : INuevoRegistro
    {
        private readonly IAspectoReadOnlyRepository _aspectoReadOnlyRepository;

        private readonly IGetItemsReadOnlyRepository _getItemsReadOnlyRepository;

        public NuevoRegistro(IAspectoReadOnlyRepository aspectoReadOnlyRepository, IGetItemsReadOnlyRepository getItemsReadOnlyRepository)
        {
            _aspectoReadOnlyRepository = aspectoReadOnlyRepository;
            _getItemsReadOnlyRepository = getItemsReadOnlyRepository;
        }

        public async Task<int> Execute(string Aspecto)
        {
            var existentes = await _getItemsReadOnlyRepository.GetAspectos();

            var existe = existentes.Find(x => x.Aspecto.Equals(Aspecto));

            if (existe != null)
            {
                return existe.Id;
            }
            else
            {
                var input = new Domain.CompromisosMCB.AspectoCMCB
                {
                    Aspecto = Aspecto
                };
                return await _aspectoReadOnlyRepository.RegistrarAspecto(input);
            }
        }
    }
}
