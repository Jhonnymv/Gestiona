using GestionAmbiental.Application.Repositories.SGA_Documentos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.SGA_Documentos.Items
{
    public class SelectItems : ISelectItems
    {
        private readonly IGetItemsReadOnlyRepository _getItemsReadOnlyRepository;

        public SelectItems(IGetItemsReadOnlyRepository getItemsReadOnlyRepository)
        {
            _getItemsReadOnlyRepository = getItemsReadOnlyRepository ?? throw new ArgumentNullException(nameof(getItemsReadOnlyRepository));
        }

        public async Task<SelectItemsOutput> Execute()
        {
            var condiciones = await _getItemsReadOnlyRepository.GetCondiciones();
            var tipos = await _getItemsReadOnlyRepository.GetTiposDocumento();
            var usuarios = await _getItemsReadOnlyRepository.GetListUsuarios();
            var output = new SelectItemsOutput();
            output.ItemsCondicion = condiciones;
            output.ItemsTipoDocumento = tipos;
            output.ItemsUsuarios = usuarios;
            return output;
        }
    }
}
