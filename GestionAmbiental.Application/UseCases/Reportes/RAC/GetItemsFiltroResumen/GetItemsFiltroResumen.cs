using GestionAmbiental.Application.Repositories.RAC;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Reportes.RAC.GetItemsFiltroResumen
{
    public class GetItemsFiltroResumen : IGetItemsFiltroResumen
    {
        private readonly IGetItemsReadOnlyRepository _getItemsReadOnlyRepository;

        public GetItemsFiltroResumen(IGetItemsReadOnlyRepository getItemsReadOnlyRepository)
        {
            _getItemsReadOnlyRepository = getItemsReadOnlyRepository ?? throw new ArgumentNullException(nameof(getItemsReadOnlyRepository));
        }

        public async Task<ItemsFiltroResumenOutput> Execute()
        {
            var output = new ItemsFiltroResumenOutput();
            output.Gerencias = await _getItemsReadOnlyRepository.GetGerencias();
            output.Empresas = await _getItemsReadOnlyRepository.GetEmpresas();
            output.Usuarios = await _getItemsReadOnlyRepository.GetUsuarios();

            return output;
        }
    }
}
