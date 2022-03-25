using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.Repositories.CompromisosMCB.GerenciaCMCB;

namespace GestionAmbiental.Application.UseCases.CompromisosMCB.GerenciaCMCB.EditarRegistro
{
    public class EditarRegistro : IEditarRegistro
    {
        private readonly IGerenciaReadOnlyRepository _gerenciaReadOnlyRepository;

        public EditarRegistro(IGerenciaReadOnlyRepository gerenciaReadOnlyRepository)
        {
            _gerenciaReadOnlyRepository = gerenciaReadOnlyRepository ?? throw new ArgumentNullException(nameof(gerenciaReadOnlyRepository));
        }

        public async Task Execute(int Id, string Gerencia, string Descripcion)
        {
            var input = new Domain.CompromisosMCB.GerenciaCMCB();
            input.Id = Id;
            input.Gerencia = Gerencia;
            input.Descripcion= Descripcion;

            await _gerenciaReadOnlyRepository.EditarGerencia(input);

        }
    }
}
