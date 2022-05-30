using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.MonitoreoCampoInterno.AvistamientoFlora.AvistamientoFlora.NuevoRegistro
{
    public interface INuevoRegistro
    {
        Task<int> Execute(
            string IdFicha,
            DateTime? FechaValidacion,
            int? IdZona,
            string? UbicacionEste,
            string? UbicacionNorte,
            string? UbicacionAltitud,
            string? Geneto,
            string? Rameto,
            string? CondicionReubicacion,
            string? Sustrato,
            Decimal? CoberturaDiametroMayor,
            Decimal? CoberturaDiametroMenor,
            Decimal? CoberturaDiametroAltura,
            string[]? Fotos
            );
    }
}
