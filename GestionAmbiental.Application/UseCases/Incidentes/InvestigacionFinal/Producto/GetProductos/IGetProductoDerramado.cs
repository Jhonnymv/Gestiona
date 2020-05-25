using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.Producto.GetProductos
{
    public interface IGetProductoDerramado
    {
        Task<ProductoDerramadoOutput> Execute(int IdProducto);
    }
}
