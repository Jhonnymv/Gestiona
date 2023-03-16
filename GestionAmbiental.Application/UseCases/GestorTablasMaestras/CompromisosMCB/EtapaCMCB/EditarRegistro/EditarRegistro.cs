using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.Repositories.CompromisosMCB.EtapaCMCB;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.CompromisosMCB.EtapaCMCB.EditarRegistro
{
    public class EditarRegistro : IEditarRegistro
    {
        private readonly IEtapaReadOnlyRepository _etapaReadOnlyRepository;

        public EditarRegistro(IEtapaReadOnlyRepository etapaReadOnlyRepository)
        {
            _etapaReadOnlyRepository = etapaReadOnlyRepository ?? throw new ArgumentNullException(nameof(etapaReadOnlyRepository));
        }

        public async Task Execute(int Id, string Etapa)
        {
            var input = new Domain.CompromisosMCB.EtapaCMCB();
            input.Id = Id;
            input.Etapa = Etapa;

            await _etapaReadOnlyRepository.EditarEtapa(input);

        }
    }
}
