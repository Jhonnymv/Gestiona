using GestionAmbiental.Application.Repositories.Incidentes.InvestigacionFinal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.Producto.GetProductos
{
    public class GetProductosDerramados : IGetProductosDerramados
    {
        private readonly IGetItemsInvestigacionFinalReadOnlyRepository _getItemsInvestigacionFinalReadOnlyRepository;

        public GetProductosDerramados(IGetItemsInvestigacionFinalReadOnlyRepository getItemsInvestigacionFinalReadOnlyRepository)
        {
            _getItemsInvestigacionFinalReadOnlyRepository = getItemsInvestigacionFinalReadOnlyRepository ?? throw new ArgumentNullException(nameof(getItemsInvestigacionFinalReadOnlyRepository));
        }

        public async Task<List<ProductoDerramadoOutput>> Execute(int IdInvestigacion)
        {
            var productos = await _getItemsInvestigacionFinalReadOnlyRepository.GetProductosDerramados(IdInvestigacion);
            var output = new List<ProductoDerramadoOutput>();
            foreach (var item in productos)
            {
                output.Add(new ProductoDerramadoOutput(item));
            }
            return output;
        }
    }
}
