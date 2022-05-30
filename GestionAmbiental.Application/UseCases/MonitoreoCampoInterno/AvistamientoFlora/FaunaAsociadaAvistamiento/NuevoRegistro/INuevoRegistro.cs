using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.MonitoreoCampoInterno.AvistamientoFlora.FaunaAsociadaAvistamiento.NuevoRegistro
{
    public interface INuevoRegistro
    {
        Task<int> Execute(
            int IdAvistamiento,
            int IdFaunaAsociada,
            int Cantidad,
            string? Descripcion
            );
    }
}
