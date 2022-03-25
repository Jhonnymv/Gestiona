using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.Repositories.CompromisosMCB.MedioVerificacionCMCB;

namespace GestionAmbiental.Application.UseCases.CompromisosMCB.MedioVerificacionCMCB.NuevoRegistro
{
    public class NuevoRegistro:INuevoRegistro
    {
        private readonly IMedioVerificacionReadOnlyRepository _medioVerificacionReadOnlyRepository;

        public NuevoRegistro(IMedioVerificacionReadOnlyRepository medioVerificacionReadOnlyRepository)
        {
            _medioVerificacionReadOnlyRepository = medioVerificacionReadOnlyRepository ?? throw new ArgumentNullException(nameof(medioVerificacionReadOnlyRepository));
        }
        public async Task<int> Execute(string Medio)
        {
            var input = new Domain.CompromisosMCB.MedioVerificacionCMCB
            {
               Medio = Medio
                
            };
            return await _medioVerificacionReadOnlyRepository.RegistrarMedioVerificacion(input);
        }
    }
}
