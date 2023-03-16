using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.CompromisosMCB.EtapaCMCB.NuevoRegistro
{
    public interface INuevoRegistro
    {
        Task<int> Execute(string Etapa);
    }
}
