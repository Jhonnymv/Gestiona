using GestionAmbiental.Application.Repositories.GestorUsuarios;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorUsuarios.GetUsuario
{
    public class GetUsuario : IGetUsuario
    {
        private readonly IGestorUsuarioReadOnlyRepository _gestorUsuarioReadOnlyRepository;

        public GetUsuario(IGestorUsuarioReadOnlyRepository gestorUsuarioReadOnlyRepository)
        {
            _gestorUsuarioReadOnlyRepository = gestorUsuarioReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorUsuarioReadOnlyRepository));
        }

        public async Task<UsuarioOutput> Execute(string usuario_red)
        {
            var usuario = await _gestorUsuarioReadOnlyRepository.GetUsuario(usuario_red);
            var output = new UsuarioOutput(usuario);
            return output;
        }
    }
}
