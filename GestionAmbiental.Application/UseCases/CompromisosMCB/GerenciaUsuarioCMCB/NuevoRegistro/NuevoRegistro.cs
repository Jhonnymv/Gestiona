using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.Repositories.CompromisosMCB.GerenciaUsuarioMCB;

namespace GestionAmbiental.Application.UseCases.CompromisosMCB.GerenciaUsuarioCMCB.NuevoRegistro
{
    public class NuevoRegistro:INuevoRegistro
    {
        private readonly IGerenciaUsuarioReadOnlyRepository _gerenciaUsuarioReadOnlyRepository;

        public NuevoRegistro(IGerenciaUsuarioReadOnlyRepository gerenciaUsuarioReadOnlyRepository)
        {
            _gerenciaUsuarioReadOnlyRepository = gerenciaUsuarioReadOnlyRepository?? throw new ArgumentNullException(nameof(gerenciaUsuarioReadOnlyRepository));
        }

        public async Task<int> Execute(int Id_Usuario, int Id_Gerencia, string Estado)
        {
            var input = new Domain.CompromisosMCB.GerenciaUsuarioCMCB
            {
                Usuario = new Domain.ValueObjects.Usuario { IdUsuario = Id_Usuario },
                Gerencia = new Domain.CompromisosMCB.GerenciaCMCB { Id = Id_Gerencia },
                Estado = Estado
            };
            
            return await _gerenciaUsuarioReadOnlyRepository.RegistraGerenciaUsuario(input);
        }
    }
}
