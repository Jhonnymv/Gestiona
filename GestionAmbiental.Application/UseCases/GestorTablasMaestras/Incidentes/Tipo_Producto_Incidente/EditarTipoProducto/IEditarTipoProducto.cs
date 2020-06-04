using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Tipo_Producto_Incidente.EditarTipoProducto
{
    public interface IEditarTipoProducto
    {
        Task Execute(int id, string descripcion, string estado);
    }
}
