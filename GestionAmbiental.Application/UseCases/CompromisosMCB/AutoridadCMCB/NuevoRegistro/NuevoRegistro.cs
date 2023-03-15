using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.Repositories.CompromisosMCB.AutoridadCMCB;
using GestionAmbiental.Application.Repositories.CompromisosMCB.CompromisoCMCB;

namespace GestionAmbiental.Application.UseCases.CompromisosMCB.AutoridadCMCB.NuevoRegistro
{
    public class NuevoRegistro : INuevoRegistro
    {
        private readonly IAutoridadReadOnlyRepository _autoridadReadOnlyRepository;
        private readonly IGetItemsReadOnlyRepository _getItemsReadOnlyRepository;

        public NuevoRegistro(IAutoridadReadOnlyRepository autoridadReadOnlyRepository, IGetItemsReadOnlyRepository getItemsReadOnlyRepository)
        {
            _autoridadReadOnlyRepository = autoridadReadOnlyRepository;
            _getItemsReadOnlyRepository = getItemsReadOnlyRepository;
        }

        public async Task<int> Execute(string Autoridad)
        {
            var existentes = await _getItemsReadOnlyRepository.GetAutoridades();

            var existe = existentes.Find(x => x.Autoridad.Equals(Autoridad));

            if (existe != null)
            {
                return existe.Id;
            }
            else
            {
                var input = new Domain.CompromisosMCB.AutoridadCMCB
                {
                    Autoridad = Autoridad,
                };
                return await _autoridadReadOnlyRepository.RegistrarAutoridad(input);
            }
        }
    }
}
