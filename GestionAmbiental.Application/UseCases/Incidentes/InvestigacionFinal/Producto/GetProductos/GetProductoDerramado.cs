using GestionAmbiental.Application.Repositories.Incidentes.InvestigacionFinal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.Producto.GetProductos
{
    public class GetProductoDerramado : IGetProductoDerramado
    {
        private readonly IGetItemsInvestigacionFinalReadOnlyRepository _getItemsInvestigacionFinalReadOnlyRepository;

        public GetProductoDerramado(IGetItemsInvestigacionFinalReadOnlyRepository getItemsInvestigacionFinalReadOnlyRepository)
        {
            _getItemsInvestigacionFinalReadOnlyRepository = getItemsInvestigacionFinalReadOnlyRepository ?? throw new ArgumentNullException(nameof(getItemsInvestigacionFinalReadOnlyRepository));
        }

        public async Task<ProductoDerramadoOutput> Execute(int IdProducto)
        {
            var producto = await _getItemsInvestigacionFinalReadOnlyRepository.GetProductoDerramado(IdProducto);
            return new ProductoDerramadoOutput(producto);
        }
    }
}
