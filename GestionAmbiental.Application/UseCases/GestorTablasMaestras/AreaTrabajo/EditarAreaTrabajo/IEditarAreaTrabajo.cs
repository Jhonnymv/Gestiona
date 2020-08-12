using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.AreaTrabajo.EditarAreaTrabajo
{
    public interface IEditarAreaTrabajo
    {
        Task Execute(int idAreaTrabajo, string descripcion, string estado);
    }
}
