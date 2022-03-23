using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.CompromisosMCB.CriterioCMBC.NuevoRegistro
{
    public interface INuevoRegistro
    {
        Task<int> Execute(string Criterio,string Detalle);
    }
}
