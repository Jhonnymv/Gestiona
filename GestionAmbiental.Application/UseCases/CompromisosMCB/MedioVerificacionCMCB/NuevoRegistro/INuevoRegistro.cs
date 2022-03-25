using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.CompromisosMCB.MedioVerificacionCMCB.NuevoRegistro
{
    public interface INuevoRegistro
    {
        Task<int> Execute(string Medio);
    }
}
