using GestionAmbiental.Application.Repositories.Incidentes.FlashReport;
using GestionAmbiental.Application.Repositories.Incidentes.ReportePreliminar;
using GestionAmbiental.Application.UseCases.GestorUsuarios.GetUsuario;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.FOCAM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.EliminarEvidencias
{
    internal class EliminarEvidencia : IEliminarEvidencia
    {
        private readonly IGetUsuario _getUsuario;
        private readonly IEditarReportePreliminarReadOnlyRepository _editarReportePreliminarReadOnlyRepository;

        public EliminarEvidencia(IGetUsuario getUsuario, IEditarReportePreliminarReadOnlyRepository editarReportePreliminarReadOnlyRepository)
        {
            _getUsuario = getUsuario ?? throw new ArgumentNullException(nameof(getUsuario));
            _editarReportePreliminarReadOnlyRepository = editarReportePreliminarReadOnlyRepository ?? throw new ArgumentNullException(nameof(editarReportePreliminarReadOnlyRepository));
        }

        public async Task<bool> Execute(int IdReporte, string NombreArchivo, string usuario_red)
        {
            var usuario = await _getUsuario.Execute(usuario_red);
            if(usuario.IdRol != (int)InspeccionRol.Admin && usuario.IdRol != (int)InspeccionRol.AsesorMAP)
                return false;
            return await _editarReportePreliminarReadOnlyRepository.EliminarEvidencia(IdReporte, NombreArchivo);
        }
    }
}
