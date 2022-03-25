using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.Repositories.CompromisosMCB.ComponenteCMCB;

namespace GestionAmbiental.Application.UseCases.CompromisosMCB.ComponentesCMCB.EditarRegistro
{
    public class EditarRegistro : IEditarRegistro
    {
        private readonly IComponeteReadOnlyRepository _componeteReadOnlyRepository;

        public EditarRegistro(IComponeteReadOnlyRepository componeteReadOnlyRepository)
        {
            _componeteReadOnlyRepository = componeteReadOnlyRepository ?? throw new ArgumentNullException(nameof(componeteReadOnlyRepository));
        }

        public async Task Execute(int Id, string Tipo, string Componente, String Servicio)
        {
            var input = new Domain.CompromisosMCB.ComponenteCMCB();
            input.Id = Id;
            input.Tipo = Tipo;
            input.Componente = Componente;
            input.Servicio = Servicio;

            await _componeteReadOnlyRepository.EditarComponente(input);

        }
    }
}
