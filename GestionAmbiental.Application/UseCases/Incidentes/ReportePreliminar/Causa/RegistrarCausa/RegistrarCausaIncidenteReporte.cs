using GestionAmbiental.Application.Repositories.Incidentes.ReportePreliminar;
using GestionAmbiental.Domain.Incidentes.InvestigacionFinal;
using GestionAmbiental.Domain.Incidentes.ReportePreliminar;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.Causa.RegistrarCausa
{
    public class RegistrarCausaIncidenteReporte : IRegistrarCausaIncidenteReporte
    {
        private readonly IRegistrarReportePreliminarReadOnlyRepository _registrarReportePreliminarReadOnlyRepository;

        public RegistrarCausaIncidenteReporte(IRegistrarReportePreliminarReadOnlyRepository registrarReportePreliminarReadOnlyRepository)
        {
            _registrarReportePreliminarReadOnlyRepository = registrarReportePreliminarReadOnlyRepository ?? throw new ArgumentNullException(nameof(registrarReportePreliminarReadOnlyRepository));
        }

        public async Task<int> Execute(int Id_Reporte, int Id_Acto_Condicion, string Tipo_Acto_Condicion, string Descripcion)
        {
            var causa = new Causa_Incidente_Reporte();
            causa.Id_Reporte_Preliminar = Id_Reporte;
            causa.Tipo_Acto_Condicion = Tipo_Acto_Condicion;
            switch (Tipo_Acto_Condicion)
            {
                case "Acto Subestándar": causa.Id_Acto_Condicion = Id_Acto_Condicion; break;
                case "Condición Subestándar": causa.Id_Acto_Condicion = Id_Acto_Condicion; break;
                case "Factor Personal": causa.Id_Factor_Causa_Incidente = Id_Acto_Condicion; break;
                case "Factor Laboral": causa.Id_Factor_Causa_Incidente = Id_Acto_Condicion; break;
            }
            causa.Descripcion = Descripcion;
            return await _registrarReportePreliminarReadOnlyRepository.RegistrarCausaIncidenteReporte(causa);
        }
    }
}
