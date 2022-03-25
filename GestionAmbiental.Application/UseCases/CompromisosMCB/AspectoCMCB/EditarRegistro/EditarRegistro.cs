using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.Repositories.CompromisosMCB.AspectoCMCB;

namespace GestionAmbiental.Application.UseCases.CompromisosMCB.AspectoCMCB.EditarRegistro
{
    public class EditarRegistro : IEditarRegistro
    {
        private readonly IAspectoReadOnlyRepository _aspectoReadOnlyRepository;

        public EditarRegistro(IAspectoReadOnlyRepository aspectoReadOnlyRepository)
        {
            _aspectoReadOnlyRepository = aspectoReadOnlyRepository ?? throw new ArgumentNullException(nameof(aspectoReadOnlyRepository));
        }

        public async Task Execute(int Id, string Aspecto)
        {
            var input = new Domain.CompromisosMCB.AspectoCMCB();
            input.Id = Id;
            input.Aspecto = Aspecto;

            await _aspectoReadOnlyRepository.EditarAspecto(input);

        }
    }
}
