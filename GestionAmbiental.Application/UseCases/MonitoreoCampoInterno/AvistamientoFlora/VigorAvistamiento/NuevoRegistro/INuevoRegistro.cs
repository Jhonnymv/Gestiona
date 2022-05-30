using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.MonitoreoCampoInterno.AvistamientoFlora.VigorAvistamiento.NuevoRegistro
{
    public interface INuevoRegistro
    {
        Task<int> Execute(
            int IdAvistamiento,
            int IdVigor,
            int Cantidad,
            string? Descripcion
            );
    }

}