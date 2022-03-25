using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.Repositories.CompromisosMCB.OrigenCMCB;

namespace GestionAmbiental.Application.UseCases.CompromisosMCB.OrigenCMCB.EditarRegistro
{
    public class EditarRegistro : IEditarRegistro
    {
        private readonly IOrigenReadOnlyRepository _origenReadOnlyRepository;

        public EditarRegistro(IOrigenReadOnlyRepository origenReadOnlyRepository)
        {
            _origenReadOnlyRepository = origenReadOnlyRepository ?? throw new ArgumentNullException(nameof(origenReadOnlyRepository));
        }

        public async Task Execute(int Id, string Origen)
        {
            var input = new Domain.CompromisosMCB.OrigenCMCB();
            input.Id = Id;
            input.Origen = Origen;

            await _origenReadOnlyRepository.EditarOrigen(input);

        }
    }
}
