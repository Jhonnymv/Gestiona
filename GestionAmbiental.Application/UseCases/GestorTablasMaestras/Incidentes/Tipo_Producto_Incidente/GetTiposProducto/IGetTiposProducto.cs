using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Tipo_Producto_Incidente.GetTiposProducto
{
    public interface IGetTiposProducto
    {
        Task<TiposProductoOutput> Execute(string filtro, int pagina, int rpp);
    }
}
