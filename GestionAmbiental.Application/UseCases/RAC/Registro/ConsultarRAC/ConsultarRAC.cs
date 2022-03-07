using GestionAmbiental.Application.Repositories.GestorUsuarios;
using GestionAmbiental.Application.Repositories.RAC;
using GestionAmbiental.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.RAC.Registro.ConsultarRAC
{
    public class ConsultarRAC : IConsultarRAC
    {
        private readonly IGetRACReadOnlyRepository _getRACReadOnlyRepository;
        private readonly IGestorUsuarioReadOnlyRepository _gestorUsuarioReadOnlyRepository;

        public ConsultarRAC(IGetRACReadOnlyRepository getRACReadOnlyRepository, IGestorUsuarioReadOnlyRepository gestorUsuarioReadOnlyRepository)
        {
            _getRACReadOnlyRepository = getRACReadOnlyRepository ?? throw new ArgumentNullException(nameof(getRACReadOnlyRepository));
            _gestorUsuarioReadOnlyRepository = gestorUsuarioReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorUsuarioReadOnlyRepository));
        }

        public async Task<RACOutput> Execute(int Id_RAC,string usuario_red)
        {
            var reporte = await _getRACReadOnlyRepository.GetRAC(Id_RAC);
            var usuario = await _gestorUsuarioReadOnlyRepository.GetUsuario(usuario_red);
            var output = new RACOutput(reporte);
            if (usuario.IdRol.Equals((int)InspeccionRol.Admin))
                output.PermisoAdmin = true;
            else
                output.PermisoAdmin = false;
            if ((usuario.IdRol.Equals((int)InspeccionRol.AsesorMAP)|| usuario.IdRol.Equals((int)InspeccionRol.AsesorSSO))&& usuario.IdUsuario.Equals(reporte.Id_Supervisor))
                output.PermisoAsesor = true;
            else
                output.PermisoAsesor = false;
            
            return output;
        }
    }
}
