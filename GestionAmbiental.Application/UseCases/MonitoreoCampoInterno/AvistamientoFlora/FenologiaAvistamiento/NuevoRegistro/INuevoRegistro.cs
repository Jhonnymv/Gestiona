using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.MonitoreoCampoInterno.AvistamientoFlora.FenologiaAvistamiento.NuevoRegistro
{
    public interface INuevoRegistro
    {
        Task<int> Execute(
            int IdAvistamiento,
            int IdFenologia,
            int Cantidad,
            string? Descripcion
            );
    }

}