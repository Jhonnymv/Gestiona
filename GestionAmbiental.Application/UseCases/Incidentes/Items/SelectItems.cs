using GestionAmbiental.Application.Repositories.Incidentes.Items;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.Items
{
    public class SelectItems : ISelectItems
    {
        private readonly IGetItemsReadOnlyRepository _getItemsReadOnlyRepository;

        public SelectItems(IGetItemsReadOnlyRepository getItemsReadOnlyRepository)
        {
            _getItemsReadOnlyRepository = getItemsReadOnlyRepository ?? throw new ArgumentNullException(nameof(getItemsReadOnlyRepository));
        }

        public async Task<SelectItemsOutput> GetSelectItems()
        {
            var output = new SelectItemsOutput();
            output.ItemsCompAmb = await _getItemsReadOnlyRepository.GetListComponenteAmbiental();
            output.ItemsEspFauna = await _getItemsReadOnlyRepository.GetListEspecieFauna();
            output.ItemsEspFlora = await _getItemsReadOnlyRepository.GetListEspecieFlora();
            output.ItemsEstadoEquipo = await _getItemsReadOnlyRepository.GetListEstadoEquipoIncidente();
            output.ItemsLugarDiposicion = await _getItemsReadOnlyRepository.GetListLugarDisposicion();
            output.ItemsNivelIncidente = await _getItemsReadOnlyRepository.GetListNivelIncidente();
            output.ItemsProductoIncidente = await _getItemsReadOnlyRepository.GetListProductoIncidente();
            output.ItemsTipoAgua = await _getItemsReadOnlyRepository.GetListTipoAgua();
            output.ItemsTipoCondicion = await _getItemsReadOnlyRepository.GetListTipoCondicion();
            output.ItemsTipoIncidente = await _getItemsReadOnlyRepository.GetListTipoIncidente();
            output.ItemsTipoProducto = await _getItemsReadOnlyRepository.GetListTipoProducotIncidente();
            output.ItemsTipoSuelo = await _getItemsReadOnlyRepository.GetListTipoSuelo();
            output.ItemsUnidadMedida = await _getItemsReadOnlyRepository.GetListUnidadMedida();
            output.ItemsGerencia = await _getItemsReadOnlyRepository.GetListGerencia();
            output.ItemsUsuarioMA = await _getItemsReadOnlyRepository.GetListUsuarioMA();
            output.ItemsUsuario = await _getItemsReadOnlyRepository.GetListUsuarios();
            output.ItemsFactores = await _getItemsReadOnlyRepository.GetListFactores();

            return output;
        }
    }
}
