using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Gestiona.ObtenerConfiguracion
{
    public interface IObtenerConfiguracion
    {
        Task<ArchivoConfiguracionOutput> Execute(string Nombre);
    }
}
