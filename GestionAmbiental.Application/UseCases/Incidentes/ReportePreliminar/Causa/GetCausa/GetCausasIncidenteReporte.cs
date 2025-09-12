using GestionAmbiental.Application.Repositories.Incidentes.Items;
using GestionAmbiental.Application.Repositories.Incidentes.ReportePreliminar;
using GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.Causa.GetCausa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.GetCausaIncidenteReporte
{
    public class GetCausasIncidenteReporte : IGetCausasIncidenteReporte
    {
        private readonly IGetItemsReportePreliminarReadOnlyRepository _getItemsReportePreliminarReadOnlyRepository;
        private readonly IGetItemsReadOnlyRepository _getItemsReadOnlyRepository;

        public GetCausasIncidenteReporte(IGetItemsReportePreliminarReadOnlyRepository getItemsReportePreliminarReadOnlyRepository, IGetItemsReadOnlyRepository getItemsReadOnlyRepository)
        {
            _getItemsReportePreliminarReadOnlyRepository = getItemsReportePreliminarReadOnlyRepository ?? throw new ArgumentNullException(nameof(getItemsReportePreliminarReadOnlyRepository));
            _getItemsReadOnlyRepository = getItemsReadOnlyRepository ?? throw new ArgumentNullException(nameof(getItemsReadOnlyRepository));
        }

        public async Task<List<CausaIncidenteReporteOutput>> Execute(int IdReporte)
        {
            var causas = await _getItemsReportePreliminarReadOnlyRepository.GetCausasReporte(IdReporte);
            causas = causas.OrderBy(x => x.Id_Acto_Condicion).OrderBy(x => x.Tipo_Acto_Condicion).ToList();
            var actos_condiciones = await _getItemsReadOnlyRepository.GetListTipoCondicion();
            var factores = await _getItemsReadOnlyRepository.GetListFactores();
            var output = new List<CausaIncidenteReporteOutput>();
            foreach(var item in causas)
            {
                var outputItem = new CausaIncidenteReporteOutput();
                outputItem.Id_Causa_Incidente = item.Id_Causa_Incidente_Reporte;
                outputItem.Id_Reporte_Preliminar = item.Id_Reporte_Preliminar;
                outputItem.Tipo_Acto_Condicion = item.Tipo_Acto_Condicion;
                outputItem.Id_Factor_Causa_Incidente = item.Id_Factor_Causa_Incidente;
                outputItem.Factor_Causa_Incidente = factores.FirstOrDefault(x => x.Id_Factor_Causa_Incidente.Equals(item.Id_Factor_Causa_Incidente))?.Descripcion;
                outputItem.Id_Acto_Condicion = item.Id_Acto_Condicion;
                outputItem.Acto_Condicion = actos_condiciones.FirstOrDefault(x => x.IdTipoCondicion.Equals(item.Id_Acto_Condicion))?.Descripcion;
                outputItem.Descripcion = item.Descripcion;
                outputItem.Estado = item.Estado;

                output.Add(outputItem);
            }
            return output;
        }
    }
}
