using GestionAmbiental.Application.Repositories.Incidentes.FlashReport;
using GestionAmbiental.Application.UseCases.GestorUsuarios.GetUsuario;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.FOCAM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.FlashReport.EliminarEvidencias
{
    internal class EliminarEvidencia : IEliminarEvidencia
    {
        private readonly IGetUsuario _getUsuario;
        private readonly IFlashReportReadOnlyRepository _flashReportReadOnlyRepository;

        public EliminarEvidencia(IGetUsuario getUsuario, IFlashReportReadOnlyRepository flashReportReadOnlyRepository)
        {
            _getUsuario = getUsuario ?? throw new ArgumentNullException(nameof(getUsuario));
            _flashReportReadOnlyRepository = flashReportReadOnlyRepository ?? throw new ArgumentNullException(nameof(flashReportReadOnlyRepository));
        }

        public async Task<bool> Execute(int IdFlashReport, string NombreArchivo, string usuario_red)
        {
            var usuario = await _getUsuario.Execute(usuario_red);
            if(usuario.IdRol != (int)InspeccionRol.Admin && usuario.IdRol != (int)InspeccionRol.AsesorMAP)
                return false;
            return await _flashReportReadOnlyRepository.EliminarEvidencia(IdFlashReport, NombreArchivo);
        }
    }
}
