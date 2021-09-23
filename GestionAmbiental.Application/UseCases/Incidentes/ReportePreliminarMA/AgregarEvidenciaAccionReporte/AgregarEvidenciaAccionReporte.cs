using GestionAmbiental.Application.Repositories.Incidentes.ReportePreliminar;
using GestionAmbiental.Domain.Incidentes.ReportePreliminar;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminarMA.AgregarEvidenciaAccionReporte
{
    public class AgregarEvidenciaAccionReporte : IAgregarEvidenciaAccionReporte
    {
        private readonly IEditarReportePreliminarReadOnlyRepository _editarReportePreliminarReadOnlyRepository;

        public AgregarEvidenciaAccionReporte(IEditarReportePreliminarReadOnlyRepository editarReportePreliminarReadOnlyRepository)
        {
            _editarReportePreliminarReadOnlyRepository = editarReportePreliminarReadOnlyRepository ?? throw new ArgumentNullException(nameof(editarReportePreliminarReadOnlyRepository));
        }

        public async Task<bool> Execute(int IdReporte, string Evidencias, string EvidenciasNombres)
        {
            var input = new Reporte_Preliminar_Incidente()
            {
                Id_Reporte_Preliminar = IdReporte,
                Evidencia = Evidencias,
                NombresArchivos = EvidenciasNombres
            };
            return await _editarReportePreliminarReadOnlyRepository.EditarEvidenciasReporte(input);
        }
    }
}
