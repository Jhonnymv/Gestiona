using GestionAmbiental.Application.Repositories.Incidentes.InvestigacionFinal;
using GestionAmbiental.Application.Repositories.Incidentes.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.Causa.GetCausa
{
    public class GetCausasIncidenteInvestigacion : IGetCausasIncidenteInvestigacion
    {
        private readonly IGetItemsInvestigacionFinalReadOnlyRepository _getItemsInvestigacionFinalReadOnlyRepository;
        private readonly IGetItemsReadOnlyRepository _getItemsReadOnlyRepository;

        public GetCausasIncidenteInvestigacion(IGetItemsInvestigacionFinalReadOnlyRepository getItemsInvestigacionFinalReadOnlyRepository, IGetItemsReadOnlyRepository getItemsReadOnlyRepository)
        {
            _getItemsInvestigacionFinalReadOnlyRepository = getItemsInvestigacionFinalReadOnlyRepository ?? throw new ArgumentNullException(nameof(getItemsInvestigacionFinalReadOnlyRepository));
            _getItemsReadOnlyRepository = getItemsReadOnlyRepository ?? throw new ArgumentNullException(nameof(getItemsReadOnlyRepository));
        }

        public async Task<List<CausaIncidenteReporteOutput>> Execute(int IdInvestigacion)
        {
            var causas = await _getItemsInvestigacionFinalReadOnlyRepository.GetCausasInvestigacion(IdInvestigacion);
            causas = causas.OrderBy(x => x.Id_Acto_Condicion).OrderBy(x => x.Tipo_Acto_Condicion).ToList();
            var actos_condiciones = await _getItemsReadOnlyRepository.GetListTipoCondicion();
            var factores = await _getItemsReadOnlyRepository.GetListFactores();
            var output = new List<CausaIncidenteReporteOutput>();
            foreach (var item in causas)
            {
                var outputItem = new CausaIncidenteReporteOutput();
                outputItem.Id_Causa_Incidente = item.Id_Causa_Incidente_Investigacion;
                outputItem.Id_Investigacion_Final = item.Id_Investigacion_Final;
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
