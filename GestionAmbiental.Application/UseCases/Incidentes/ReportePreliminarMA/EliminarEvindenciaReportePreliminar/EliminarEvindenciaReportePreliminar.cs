using GestionAmbiental.Application.Repositories.Incidentes.ReportePreliminar;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminarMA.EliminarEvindenciaReportePreliminar
{
    public class EliminarEvindenciaReportePreliminar : IEliminarEvindenciaReportePreliminar
    {
        private readonly IEditarReportePreliminarReadOnlyRepository _editarReportePreliminarReadOnlyRepository;

        public EliminarEvindenciaReportePreliminar(IEditarReportePreliminarReadOnlyRepository editarReportePreliminarReadOnlyRepository)
        {
            _editarReportePreliminarReadOnlyRepository = editarReportePreliminarReadOnlyRepository;
        }

        public async Task Execute(int IdReporte, string NombreArchivo)
        {
            await _editarReportePreliminarReadOnlyRepository.EliminarEvidencia(IdReporte, NombreArchivo);
        }
    }
}
