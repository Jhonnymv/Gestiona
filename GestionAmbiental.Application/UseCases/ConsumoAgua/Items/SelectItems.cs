using GestionAmbiental.Application.Repositories.ConsumoAgua;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.ConsumoAgua.Items
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
            var resoluciones = await _getItemsReadOnlyRepository.GetReslucionesConsumoAgua();
            var output = new SelectItemsOutput();
            output.ItemsResoluciones = resoluciones;
            return output;
        }
    }
}
