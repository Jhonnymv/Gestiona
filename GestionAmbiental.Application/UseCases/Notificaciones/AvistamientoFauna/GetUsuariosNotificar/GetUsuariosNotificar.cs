using GestionAmbiental.Application.Repositories.AvistamientoFauna;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Notificaciones.AvistamientoFauna.GetUsuariosNotificar
{
    public class GetUsuariosNotificar : IGetUsuariosNotificar
    {
        private readonly IGetUsuariosNotificarReadOnlyRepository _getUsuariosNotificarReadOnlyRepository;

        public GetUsuariosNotificar(IGetUsuariosNotificarReadOnlyRepository getUsuariosNotificarReadOnlyRepository)
        {
            _getUsuariosNotificarReadOnlyRepository = getUsuariosNotificarReadOnlyRepository ?? throw new ArgumentNullException(nameof(getUsuariosNotificarReadOnlyRepository));
        }

        public async Task<List<UsuarioOutput>> Execute()
        {
            var usuario = await _getUsuariosNotificarReadOnlyRepository.GetUsuariosNotificarIncidente();

            var usuariosOutput = new List<UsuarioOutput>();
            foreach (var item in usuario)
            {
                usuariosOutput.Add(new UsuarioOutput(item));
            }
            return usuariosOutput;
        }
    }
}
