using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.AreaTrabajo.AgregarAreaTrabajo
{
    public interface IAgregarAreaTrabajo
    {
        Task<int> Execute(string descripcion);
    }
}
