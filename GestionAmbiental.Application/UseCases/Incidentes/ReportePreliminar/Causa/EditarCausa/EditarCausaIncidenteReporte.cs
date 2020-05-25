using GestionAmbiental.Application.Repositories.Incidentes.ReportePreliminar;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.Incidentes.ReportePreliminar;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.Causa.EditarCausa
{
    public class EditarCausaIncidenteReporte : IEditarCausaIncidenteReporte
    {
        private readonly IEditarReportePreliminarReadOnlyRepository _editarReportePreliminarReadOnlyRepository;

        public EditarCausaIncidenteReporte(IEditarReportePreliminarReadOnlyRepository editarReportePreliminarReadOnlyRepository)
        {
            _editarReportePreliminarReadOnlyRepository = editarReportePreliminarReadOnlyRepository ?? throw new ArgumentNullException(nameof(editarReportePreliminarReadOnlyRepository));
        }

        public async Task<int> Execute(int Id_Causa_Incidente, int Id_Reporte, int Id_Acto_Condicion, string Tipo_Acto_Condicion)
        {
            var input = new Causa_Incidente_Reporte();
            input.Id_Causa_Incidente_Reporte = Id_Causa_Incidente;
            input.Id_Reporte_Preliminar = Id_Reporte;
            input.Id_Acto_Condicion = Id_Acto_Condicion;
            input.Tipo_Acto_Condicion = Tipo_Acto_Condicion;
            input.Estado = EstadosIncidentes.Registrado.ToString();
            return await _editarReportePreliminarReadOnlyRepository.EditarCausaIncidenteReporte(input);
        }
    }
}
