using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.Repositories.MonitoreoCampoInterno.AvistamientoFlora.FloraAsociadaAvistamiento;

namespace GestionAmbiental.Application.UseCases.MonitoreoCampoInterno.AvistamientoFlora.FloraAsociadaAvistamiento.NuevoRegistro
{
    public class NuevoRegistro: INuevoRegistro
    {
        private readonly IFloraAsociadaAvistamientoReadOnlyRepository _floraAsociadaAvistamientoReadOnlyRepository;

        public NuevoRegistro(IFloraAsociadaAvistamientoReadOnlyRepository floraAsociadaAvistamientoReadOnlyRepository)
        {
            _floraAsociadaAvistamientoReadOnlyRepository = floraAsociadaAvistamientoReadOnlyRepository;
        }

        public async Task<int> Execute(int IdAvistamiento, int IdFloraAsociada, int Cantidad, string Descripcion)
        {
            var input = new Domain.MonitoreoCampoInterno.AvistamientoFlora.FloraAsociadaAvistamiento();
            input.Avistamiento = new Domain.MonitoreoCampoInterno.AvistamientoFlora.AvistamientoFlora { Id = IdAvistamiento };
            input.FloraAsociada = new Domain.MonitoreoCampoInterno.AvistamientoFlora.AvistamientoFlora { Id = IdFloraAsociada };
            input.Cantidad = Cantidad;
            input.Descripcion = Descripcion;

            var idFloraAsociadaAvistamiento = await _floraAsociadaAvistamientoReadOnlyRepository.RegistrarFloraAsociadaAvistamiento(input);
            return idFloraAsociadaAvistamiento;
        }
    }
}
