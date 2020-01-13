using GestionAmbiental.Application.Repositories.GestorUsuarios;
using GestionAmbiental.Application.Repositories.RAC;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.RAC.Verificacion.ConsultarRAC
{
    public class ConsultarRAC : IConsultarRAC
    {
        private readonly IGetRACReadOnlyRepository _getRACReadOnlyRepository;
        private readonly IGestorUsuarioReadOnlyRepository _gestorUsuarioReadOnlyRepository;

        public ConsultarRAC(
            IGetRACReadOnlyRepository getRACReadOnlyRepository,
            IGestorUsuarioReadOnlyRepository gestorUsuarioReadOnlyRepository
            )
        {
            _getRACReadOnlyRepository = getRACReadOnlyRepository ?? throw new ArgumentNullException(nameof(getRACReadOnlyRepository));
            _gestorUsuarioReadOnlyRepository = gestorUsuarioReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorUsuarioReadOnlyRepository));
        }

        public async Task<RACOutput> Execute(int Id_RAC,string usuario_red)
        {
            var reporte = await _getRACReadOnlyRepository.GetRAC(Id_RAC);
            var usuario = await _gestorUsuarioReadOnlyRepository.GetIdUsuario(usuario_red);
            var output = new RACOutput(reporte)
            {
                Permiso = reporte.Supervisor.IdUsuario.Equals(usuario)
            };
            return output;
        }
    }
}
