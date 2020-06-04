using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Usuario_Notificar_Incidente;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Usuarios_Notificar_Incidente.GetUsuarioIncidente
{
    public class GetUsuarioIncidente : IGetUsuarioIncidente
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public GetUsuarioIncidente(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<UsuarioOutput> Execute(int idUsuario)
        {
            var usuario = await _gestorTablasMaestrasReadOnlyRepository.GetUsuario_Notificar_Incidente(idUsuario);
            var output = new UsuarioOutput(usuario);
            return output;
        }
    }
}
