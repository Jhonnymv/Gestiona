using GestionAmbiental.Application.Repositories.MonitoreoCampoInterno.CalidadAgua;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.MonitoreoCampoInterno.CalidadAgua.Items
{
    public class SelectItems : ISelectItems
    {
        private readonly IGetItemsReadOnlyRepository _getItemsReadOnlyRepository;

        public SelectItems(IGetItemsReadOnlyRepository getItemsReadOnlyRepository)
        {
            _getItemsReadOnlyRepository = getItemsReadOnlyRepository ?? throw new ArgumentNullException(nameof(getItemsReadOnlyRepository));
        }

        public async Task<SelectItemsOuput> Execute()
        {
            var usuarios = await _getItemsReadOnlyRepository.GetUsuarios();

            var output=new SelectItemsOuput();

            output.ItemsUsuario= usuarios;

            return output;
        }
    }
}
