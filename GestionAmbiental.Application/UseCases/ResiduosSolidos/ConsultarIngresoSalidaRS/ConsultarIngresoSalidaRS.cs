using GestionAmbiental.Application.Repositories.GestorUsuarios;
using GestionAmbiental.Application.Repositories.ResiduosSolidos;
using GestionAmbiental.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.ResiduosSolidos.ConsultarIngresoSalidaRS
{
    public class ConsultarIngresoSalidaRS : IConsultarIngresoSalidaRS
    {
        private readonly IGetIngresoSalidaRSReadOnlyRepository _getIngresoSalidaRSReadOnlyRepository;
        private readonly IGestorUsuarioReadOnlyRepository _gestorUsuarioReadOnlyRepository;

        public ConsultarIngresoSalidaRS(IGetIngresoSalidaRSReadOnlyRepository getIngresoSalidaRSReadOnlyRepository, IGestorUsuarioReadOnlyRepository gestorUsuarioReadOnlyRepository)
        {
            _getIngresoSalidaRSReadOnlyRepository = getIngresoSalidaRSReadOnlyRepository ?? throw new ArgumentNullException(nameof(getIngresoSalidaRSReadOnlyRepository));
            _gestorUsuarioReadOnlyRepository = gestorUsuarioReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorUsuarioReadOnlyRepository));
        }

        public async Task<IngresoSalidaRSOutput> Execute(int Id_Ingreso_Salida_RS,string usuario_red)
        {
            var data = await _getIngresoSalidaRSReadOnlyRepository.GetIngreso_Salida_RS(Id_Ingreso_Salida_RS);
            var usuario = await _gestorUsuarioReadOnlyRepository.GetUsuario(usuario_red);
            var output = new IngresoSalidaRSOutput(data);
            if (data.Id_Registrado_Por.Equals(usuario.IdUsuario) || usuario.IdRol.Equals((int)RSRoles.AsesorMAP))
                output.Permiso = true;
            else
                output.Permiso = false;
            return output;
        }
    }
}
