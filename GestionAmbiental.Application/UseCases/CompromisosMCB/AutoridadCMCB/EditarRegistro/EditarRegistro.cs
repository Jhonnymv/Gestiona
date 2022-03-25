using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.Repositories.CompromisosMCB.AutoridadCMCB;

namespace GestionAmbiental.Application.UseCases.CompromisosMCB.AutoridadCMCB.EditarRegistro
{
    public class EditarRegistro : IEditarRegistro
    {
        private readonly IAutoridadReadOnlyRepository _autoridadReadOnlyRepository;

        public EditarRegistro(IAutoridadReadOnlyRepository autoridadReadOnlyRepository)
        {
            _autoridadReadOnlyRepository = autoridadReadOnlyRepository ?? throw new ArgumentNullException(nameof(autoridadReadOnlyRepository));
        }

        public async Task Execute(int Id, string Autoridad)
        {
            var input = new Domain.CompromisosMCB.AutoridadCMCB();
            input.Id = Id;
            input.Autoridad = Autoridad;
            
            await _autoridadReadOnlyRepository.EditarAutoridad(input);

        }
    }
}
