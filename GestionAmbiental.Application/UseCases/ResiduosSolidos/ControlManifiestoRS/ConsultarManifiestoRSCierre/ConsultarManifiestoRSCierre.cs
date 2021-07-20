using GestionAmbiental.Application.Repositories.GestorUsuarios;
using GestionAmbiental.Application.Repositories.ResiduosSolidos.ControlManifiestoRS;
using GestionAmbiental.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.ResiduosSolidos.ControlManifiestoRS.ConsultarManifiestoRSCierre
{
    public class ConsultarManifiestoRSCierre : IConsultarManifiestoRSCierre
    {
        private readonly IGetManifiestosRSReadOnlyRepository _getManifiestosRSReadOnlyRepository;
        private readonly IGestorUsuarioReadOnlyRepository _gestorUsuarioReadOnlyRepository;

        public ConsultarManifiestoRSCierre(IGetManifiestosRSReadOnlyRepository getManifiestosRSReadOnlyRepository, IGestorUsuarioReadOnlyRepository gestorUsuarioReadOnlyRepository)
        {
            _getManifiestosRSReadOnlyRepository = getManifiestosRSReadOnlyRepository ?? throw new ArgumentNullException(nameof(getManifiestosRSReadOnlyRepository));
            _gestorUsuarioReadOnlyRepository = gestorUsuarioReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorUsuarioReadOnlyRepository));
        }

        public async Task<ManifiestoRSOutput> Execute(int Id_Manifiesto, string usuario_red)
        {
            var data = await _getManifiestosRSReadOnlyRepository.GetManifiesto_RS_Completo(Id_Manifiesto);
            var usuario = await _gestorUsuarioReadOnlyRepository.GetUsuario(usuario_red);
            var output = new ManifiestoRSOutput(data);
            if (usuario.IdRol.Equals((int)RSRoles.AsesorMAP)|| usuario.IdRol.Equals((int)RSRoles.Admin))
                output.Permiso = true;
            else
                output.Permiso = false;
            return output;
        }
    }
}
