using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.Producto.GetProductos
{
    public interface IGetProductosDerramados
    {
        Task<List<ProductoDerramadoOutput>> Execute(int IdInvestigacion);
    }
}
