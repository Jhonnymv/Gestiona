using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Producto_Incidente.AgregarProductoIncidente
{
    public interface IAgregarProductoIncidente
    {
        Task<int> Execute(string descripcion);
    }
}
