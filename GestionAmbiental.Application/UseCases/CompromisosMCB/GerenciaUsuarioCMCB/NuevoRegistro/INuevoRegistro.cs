using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.CompromisosMCB.GerenciaUsuarioCMCB.NuevoRegistro
{
    public interface INuevoRegistro
    {
        Task<int> Execute(
            int Id_Usuario,
            int Id_Gerencia,
            string Estado);
    }
}
