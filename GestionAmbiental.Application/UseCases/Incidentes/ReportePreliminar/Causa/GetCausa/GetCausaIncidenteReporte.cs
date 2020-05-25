using GestionAmbiental.Application.Repositories.Incidentes.Items;
using GestionAmbiental.Application.Repositories.Incidentes.ReportePreliminar;
using GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.GetCausaIncidenteReporte;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.Causa.GetCausa
{
    public class GetCausaIncidenteReporte : IGetCausaIncidenteReporte
    {
        private readonly IGetItemsReportePreliminarReadOnlyRepository _getItemsReportePreliminarReadOnlyRepository;
        private readonly IGetItemsReadOnlyRepository _getItemsReadOnlyRepository;

        public GetCausaIncidenteReporte(IGetItemsReportePreliminarReadOnlyRepository getItemsReportePreliminarReadOnlyRepository, IGetItemsReadOnlyRepository getItemsReadOnlyRepository)
        {
            _getItemsReportePreliminarReadOnlyRepository = getItemsReportePreliminarReadOnlyRepository ?? throw new ArgumentNullException(nameof(getItemsReportePreliminarReadOnlyRepository));
            _getItemsReadOnlyRepository = getItemsReadOnlyRepository ?? throw new ArgumentNullException(nameof(getItemsReadOnlyRepository));
        }

        public async Task<CausaIncidenteReporteOutput> Execute(int IdCausa)
        {
            var causa = await _getItemsReportePreliminarReadOnlyRepository.GetCausaReporte(IdCausa);
            var actos_condiciones = await _getItemsReadOnlyRepository.GetListTipoCondicion();

            var output = new CausaIncidenteReporteOutput();
            output.Id_Causa_Incidente = causa.Id_Causa_Incidente_Reporte;
            output.Id_Reporte_Preliminar = causa.Id_Reporte_Preliminar;
            output.Id_Acto_Condicion = causa.Id_Acto_Condicion;
            output.Acto_Condicion = actos_condiciones.FirstOrDefault(x => x.IdTipoCondicion.Equals(causa.Id_Acto_Condicion)).Descripcion;
            output.Tipo_Acto_Condicion = causa.Tipo_Acto_Condicion;
            output.Estado = causa.Estado;

            return output;
        }
    }
}
