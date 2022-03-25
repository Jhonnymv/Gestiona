using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.Repositories.CompromisosMCB.GerenciaCMCB;

namespace GestionAmbiental.Application.UseCases.CompromisosMCB.GerenciaCMCB.NuevoRegistro
{
    public class NuevoRegistro:INuevoRegistro
    {
        private readonly IGerenciaReadOnlyRepository _gerenciaReadOnlyRepository;

        public NuevoRegistro(IGerenciaReadOnlyRepository gerenciaReadOnlyRepository)
        {
            _gerenciaReadOnlyRepository = gerenciaReadOnlyRepository ?? throw new ArgumentNullException(nameof(gerenciaReadOnlyRepository));
        }
        public async Task<int> Execute(string Gerencia, string Descripcion)
        {
            var input = new Domain.CompromisosMCB.GerenciaCMCB
            {
                Gerencia = Gerencia,
                Descripcion = Descripcion
                
            };
            return await _gerenciaReadOnlyRepository.RegistrarGerencia(input);
        }
    }
}
