using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.Repositories.CompromisosMCB.MedioVerificacionCMCB;

namespace GestionAmbiental.Application.UseCases.CompromisosMCB.MedioVerificacionCMCB.EditarRegistro
{
    public class EditarRegistro : IEditarRegistro
    {
        private readonly IMedioVerificacionReadOnlyRepository _medioVerificacionReadOnlyRepository;

        public EditarRegistro(IMedioVerificacionReadOnlyRepository medioVerificacionReadOnlyRepository)
        {
            _medioVerificacionReadOnlyRepository = medioVerificacionReadOnlyRepository ?? throw new ArgumentNullException(nameof(medioVerificacionReadOnlyRepository));
        }

        public async Task Execute(int Id, string Medio)
        {
            var input = new Domain.CompromisosMCB.MedioVerificacionCMCB();
            input.Id = Id;
            input.Medio = Medio;

            await _medioVerificacionReadOnlyRepository.EditarMedioVerificacion(input);

        }
    }
}
