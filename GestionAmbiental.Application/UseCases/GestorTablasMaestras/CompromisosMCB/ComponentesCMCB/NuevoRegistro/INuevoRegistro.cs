using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.CompromisosMCB.ComponentesCMCB.NuevoRegistro
{
    public interface INuevoRegistro
    {
        Task<int> Execute(string Tipo, string Componente, string Servicio);
    }
}
