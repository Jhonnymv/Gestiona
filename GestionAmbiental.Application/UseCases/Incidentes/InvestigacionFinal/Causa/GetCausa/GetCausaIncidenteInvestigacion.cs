using GestionAmbiental.Application.Repositories.Incidentes.InvestigacionFinal;
using GestionAmbiental.Application.Repositories.Incidentes.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.Causa.GetCausa
{
    public class GetCausaIncidenteInvestigacion : IGetCausaIncidenteInvestigacion
    {
        private readonly IGetItemsInvestigacionFinalReadOnlyRepository _getItemsInvestigacionFinalReadOnlyRepository;
        private readonly IGetItemsReadOnlyRepository _getItemsReadOnlyRepository;

        public GetCausaIncidenteInvestigacion(IGetItemsInvestigacionFinalReadOnlyRepository getItemsInvestigacionFinalReadOnlyRepository, IGetItemsReadOnlyRepository getItemsReadOnlyRepository)
        {
            _getItemsInvestigacionFinalReadOnlyRepository = getItemsInvestigacionFinalReadOnlyRepository ?? throw new ArgumentNullException(nameof(getItemsInvestigacionFinalReadOnlyRepository));
            _getItemsReadOnlyRepository = getItemsReadOnlyRepository ?? throw new ArgumentNullException(nameof(getItemsReadOnlyRepository));
        }

        public async Task<CausaIncidenteReporteOutput> Execute(int IdCausa)
        {
            var causa = await _getItemsInvestigacionFinalReadOnlyRepository.GetCausaInvestigacion(IdCausa);
            var actos_condiciones = await _getItemsReadOnlyRepository.GetListTipoCondicion();

            var output = new CausaIncidenteReporteOutput();
            output.Id_Causa_Incidente = causa.Id_Causa_Incidente_Investigacion;
            output.Id_Investigacion_Final = causa.Id_Investigacion_Final;
            output.Id_Acto_Condicion = causa.Id_Acto_Condicion;
            output.Tipo_Acto_Condicion = causa.Tipo_Acto_Condicion;
            output.Id_Factor_Causa_Incidente = causa.Id_Factor_Causa_Incidente;
            //output.Descripcion = actos_condiciones.FirstOrDefault(x => x.IdTipoCondicion.Equals(causa.Id_Acto_Condicion)).Descripcion;
            output.Descripcion = causa.Descripcion;
            output.Estado = causa.Estado;

            return output;
        }
    }
}
