using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Producto_Incidente.GetProductoIncidente
{
    public interface IGetProductoIncidente
    {
        Task<ProductoIncidenteOutput> Execute(int id);
    }
}
