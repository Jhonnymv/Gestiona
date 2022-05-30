using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.MonitoreoCampoInterno.AvistamientoFlora.FloraAsociadaAvistamiento.NuevoRegistro
{
    public interface INuevoRegistro
    {
        Task<int> Execute(
            int IdAvistamiento,
            int IdFloraAsociada,
            int Cantidad,
            string? Descripcion
            );
    }

}