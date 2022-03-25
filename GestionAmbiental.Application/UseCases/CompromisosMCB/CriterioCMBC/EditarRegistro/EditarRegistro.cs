using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.Repositories.CompromisosMCB.CriterioCMCB;

namespace GestionAmbiental.Application.UseCases.CompromisosMCB.CriterioCMBC.EditarRegistro
{
    public class EditarRegistro : IEditarRegistro
    {
        private readonly ICriterioReadOnlyRepository _criterioReadOnlyRepository;

        public EditarRegistro(ICriterioReadOnlyRepository criterioReadOnlyRepository)
        {
            _criterioReadOnlyRepository = criterioReadOnlyRepository ?? throw new ArgumentNullException(nameof(criterioReadOnlyRepository));
        }

        public async Task Execute(int Id, string Criterio, string Detalle)
        {
            var input = new Domain.CompromisosMCB.CriterioCMCB();
            input.Id = Id;
            input.Criterio = Criterio;
            input.Detalle = Detalle;

            await _criterioReadOnlyRepository.EditarCriterio(input);

        }
    }
}
