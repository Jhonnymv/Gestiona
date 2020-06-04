using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Producto_Incidente.GetProductosIncidente
{
    public interface IGetProductosIncidente
    {
        Task<ProductosIncidenteOutput> Execute(string filtro, int pagina, int rpp);
    }
}
