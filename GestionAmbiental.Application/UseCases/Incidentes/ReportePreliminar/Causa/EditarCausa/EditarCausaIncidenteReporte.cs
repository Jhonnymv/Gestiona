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

        public async Task<int> Execute(int Id_Causa_Incidente, int Id_Reporte, int Id_Acto_Condicion, string Tipo_Acto_Condicion, int Id_Factor_Causa, string Descripcion)
        {
            var input = new Causa_Incidente_Reporte();
            input.Id_Causa_Incidente_Reporte = Id_Causa_Incidente;
            input.Id_Reporte_Preliminar = Id_Reporte;
            input.Tipo_Acto_Condicion = Tipo_Acto_Condicion;
            switch (Tipo_Acto_Condicion)
            {
                case "Acto Subestándar": input.Id_Acto_Condicion = Id_Acto_Condicion; break;
                case "Condición Subestándar": input.Id_Acto_Condicion = Id_Acto_Condicion; break;
                case "Factor Personal": input.Id_Factor_Causa_Incidente = Id_Factor_Causa; break;
                case "Factor Laboral": input.Id_Factor_Causa_Incidente = Id_Factor_Causa; break;
            }
            input.Descripcion = Descripcion;
            input.Estado = EstadosIncidentes.Registrado.ToString();
            return await _editarReportePreliminarReadOnlyRepository.EditarCausaIncidenteReporte(input);
        }
    }
}
