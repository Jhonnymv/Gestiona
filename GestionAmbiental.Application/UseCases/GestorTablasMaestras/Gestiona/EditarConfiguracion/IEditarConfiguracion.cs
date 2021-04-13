using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Gestiona.EditarConfiguracion
{
    public interface IEditarConfiguracion
    {
        Task Exectue(string nombre, string ruta, string texto);
    }
}
