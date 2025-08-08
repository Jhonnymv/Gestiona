using GestionAmbiental.Application.Repositories.GestorUsuarios;
using GestionAmbiental.Application.Repositories.Incidentes.ReportePreliminar;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.GetReportePreliminar
{
    public class GetReportePreliminar : IGetReportePreliminar
    {
        private readonly IGetReportePreliminarReadOnlyRepository _getReportePreliminarReadOnlyRepository;
        private readonly IGestorUsuarioReadOnlyRepository _gestorUsuarioReadOnlyRepository;
        public GetReportePreliminar(IGetReportePreliminarReadOnlyRepository getReportePreliminarReadOnlyRepository, IGestorUsuarioReadOnlyRepository gestorUsuarioReadOnlyRepository)
        {
            _getReportePreliminarReadOnlyRepository = getReportePreliminarReadOnlyRepository ?? throw new ArgumentNullException(nameof(getReportePreliminarReadOnlyRepository));
            _gestorUsuarioReadOnlyRepository = gestorUsuarioReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorUsuarioReadOnlyRepository));
        }
        public async Task<ReportePreliminarOutput> Execute(int IdReporte)
        {
            var reporte = await _getReportePreliminarReadOnlyRepository.GetReporte(IdReporte);
            var output = new ReportePreliminarOutput(reporte);

            output.Investigacion = await _getReportePreliminarReadOnlyRepository.GetInvestigacionReporte(IdReporte);
            return output;
        }
        public async Task<ReportePreliminarOutput> Execute(int IdReporte, string usuario_red)
        {
            var usuario = await _gestorUsuarioReadOnlyRepository.GetUsuario(usuario_red);

            var reporte = await _getReportePreliminarReadOnlyRepository.GetReporte(IdReporte);
            var output = new ReportePreliminarOutput(reporte);
            if (output.Id_Registrado_Por == usuario.IdUsuario)
                output.PermisoEditar = true;

            output.Investigacion = await _getReportePreliminarReadOnlyRepository.GetInvestigacionReporte(IdReporte);
            return output;
        }
    }
}
