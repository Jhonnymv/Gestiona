using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Producto_Incidente.EditarProductoIncidente
{
    public interface IEditarProductoIncidente
    {
        Task Execute(int id, string descripcion, string estado);
    }
}
