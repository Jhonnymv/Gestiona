using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Tipo_Producto_Incidente.AgregarTipoProducto
{
    public interface IAgregarTipoProducto
    {
        Task<int> Execute(string descripcion);
    }
}
