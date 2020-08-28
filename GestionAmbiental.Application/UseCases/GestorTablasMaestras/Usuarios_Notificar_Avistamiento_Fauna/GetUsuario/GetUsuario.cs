using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Usuarios_Notificar_Avistamiento_Fauna.GetUsuario
{
    public class GetUsuario : IGetUsuario
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public GetUsuario(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<UsuarioOutput> Execute(int idUsuario)
        {
            var usuario = await _gestorTablasMaestrasReadOnlyRepository.GetUsuario_Notificar_AF(idUsuario);
            var output = new UsuarioOutput(usuario);
            return output;
        }
    }
}
