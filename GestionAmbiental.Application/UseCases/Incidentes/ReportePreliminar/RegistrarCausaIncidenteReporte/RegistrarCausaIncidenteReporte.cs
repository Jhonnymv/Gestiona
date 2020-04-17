using GestionAmbiental.Application.Repositories.Incidentes.ReportePreliminar;
using GestionAmbiental.Domain.Incidentes.InvestigacionFinal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.RegistrarCausaIncidenteReporte
{
    public class RegistrarCausaIncidenteReporte : IRegistrarCausaIncidenteReporte
    {
        private readonly IRegistrarReportePreliminarReadOnlyRepository _registrarReportePreliminarReadOnlyRepository;

        public RegistrarCausaIncidenteReporte(IRegistrarReportePreliminarReadOnlyRepository registrarReportePreliminarReadOnlyRepository)
        {
            _registrarReportePreliminarReadOnlyRepository = registrarReportePreliminarReadOnlyRepository ?? throw new ArgumentNullException(nameof(registrarReportePreliminarReadOnlyRepository));
        }

        public async Task<int> Execute(int Id_Reporte, int Id_Acto_Condicion,string Tipo_Acto_Condicion)
        {
            var causa = new Causa_Incidente();
            causa.Id_Reporte_Preliminar = Id_Reporte;
            causa.Id_Acto_Condicion = Id_Acto_Condicion;
            causa.Tipo_Acto_Condicion = Tipo_Acto_Condicion;
            return await _registrarReportePreliminarReadOnlyRepository.RegistrarCausaIncidenteReporte(causa);
        }
    }
}
