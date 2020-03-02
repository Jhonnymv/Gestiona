using GestionAmbiental.Application.Repositories.GestorUsuarios;
using GestionAmbiental.Application.Repositories.ResiduosSolidos.ControlManifiestoRS;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.ResiduosSolidos.ControlManifiestoRS.ConsultarManifiestoRS
{
    public class ConsultarManifiestoRS : IConsultarManifiestoRS
    {
        private readonly IGetManifiestosRSReadOnlyRepository _getManifiestosRSReadOnlyRepository;
        private readonly IGestorUsuarioReadOnlyRepository _gestorUsuarioReadOnlyRepository;

        public ConsultarManifiestoRS(IGetManifiestosRSReadOnlyRepository getManifiestosRSReadOnlyRepository, IGestorUsuarioReadOnlyRepository gestorUsuarioReadOnlyRepository)
        {
            _getManifiestosRSReadOnlyRepository = getManifiestosRSReadOnlyRepository ?? throw new ArgumentNullException(nameof(getManifiestosRSReadOnlyRepository));
            _gestorUsuarioReadOnlyRepository = gestorUsuarioReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorUsuarioReadOnlyRepository));
        }

        public async Task<ManifiestoRSOutput> Execute(int Id_Manifiesto, string usuario_red)
        {
            var data = await _getManifiestosRSReadOnlyRepository.GetManifiestoRS(Id_Manifiesto);
            var usuario = await _gestorUsuarioReadOnlyRepository.GetUsuario(usuario_red);
            var output = new ManifiestoRSOutput(data);
            if (data.Id_Registrado_Por.Equals(usuario.IdUsuario))
                output.Permiso = true;
            else
                output.Permiso = false;
            return output;
        }
    }
}
