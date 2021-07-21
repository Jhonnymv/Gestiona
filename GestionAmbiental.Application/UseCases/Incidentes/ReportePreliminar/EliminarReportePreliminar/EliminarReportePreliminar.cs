using GestionAmbiental.Application.Repositories.Incidentes.ReportePreliminar;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.EliminarReportePreliminar
{
    public class EliminarReportePreliminar : IEliminarReportePreliminar
    {
        private readonly IEditarReportePreliminarReadOnlyRepository _editarReportePreliminarReadOnlyRepository;

        public EliminarReportePreliminar(IEditarReportePreliminarReadOnlyRepository editarReportePreliminarReadOnlyRepository)
        {
            _editarReportePreliminarReadOnlyRepository = editarReportePreliminarReadOnlyRepository ?? throw new ArgumentNullException(nameof(editarReportePreliminarReadOnlyRepository));
        }

        public async Task<int> Execute(int IdReportePreliminar)
        {
            return await _editarReportePreliminarReadOnlyRepository.EliminarReportePreliminar(IdReportePreliminar);
        }
    }
}
