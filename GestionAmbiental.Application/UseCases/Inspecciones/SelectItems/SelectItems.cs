using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.Repositories.Inspecciones;
using GestionAmbiental.Domain.Inspecciones;

namespace GestionAmbiental.Application.UseCases.Inspecciones.SelectItems
{
    public class SelectItems : ISelectItems
    {
        private readonly IGetItemsReadOnlyRepository _getItemsReadOnlyRepository;
        public SelectItems(IGetItemsReadOnlyRepository getItemsReadOnlyRepository)
        {
            _getItemsReadOnlyRepository = getItemsReadOnlyRepository;
        }
        public async Task<ItemsSelectResumen> Execute()
        {
            var items = await _getItemsReadOnlyRepository.GetItemsResumen();
            return  items;
        }
    }
}
