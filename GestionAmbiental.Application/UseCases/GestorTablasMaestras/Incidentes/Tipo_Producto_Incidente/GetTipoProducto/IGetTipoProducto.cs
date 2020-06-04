using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Tipo_Producto_Incidente.GetTipoProducto
{
    public interface IGetTipoProducto
    {
        Task<TipoProductoOutput> Execute(int id);
    }
}
