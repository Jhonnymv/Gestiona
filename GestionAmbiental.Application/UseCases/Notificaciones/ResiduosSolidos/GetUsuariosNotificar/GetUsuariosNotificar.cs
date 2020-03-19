using GestionAmbiental.Application.Repositories.ResiduosSolidos.ControlManifiestoRS;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Notificaciones.ResiduosSolidos.GetUsuariosNotificar
{
    public class GetUsuariosNotificar : IGetUsuariosNotificar
    {
        private readonly IGetUsuarioManifiestoRSReadOnlyRepository _getUsuarioManifiestoRSReadOnlyRepository;

        public GetUsuariosNotificar(IGetUsuarioManifiestoRSReadOnlyRepository getUsuarioManifiestoRSReadOnlyRepository)
        {
            _getUsuarioManifiestoRSReadOnlyRepository = getUsuarioManifiestoRSReadOnlyRepository ?? throw new ArgumentNullException(nameof(getUsuarioManifiestoRSReadOnlyRepository));
        }
        public async Task<List<UsuarioNotificarOutput>> Execute()
        {
            var usuarios = await _getUsuarioManifiestoRSReadOnlyRepository.GetUsuariosNotificarRS();
            var output = new List<UsuarioNotificarOutput>();
            foreach (var item in usuarios)
            {
                output.Add(new UsuarioNotificarOutput(item));
            }
            return output;
        }
    }
}
