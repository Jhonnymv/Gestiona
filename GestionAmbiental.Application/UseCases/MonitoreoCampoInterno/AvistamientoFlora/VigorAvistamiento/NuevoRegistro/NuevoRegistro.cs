using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.Repositories.MonitoreoCampoInterno.AvistamientoFlora.VigorAvistamiento;

namespace GestionAmbiental.Application.UseCases.MonitoreoCampoInterno.AvistamientoFlora.VigorAvistamiento.NuevoRegistro
{
    public class NuevoRegistro: INuevoRegistro
    {
        private readonly IVigorAvistamientoReadOnlyRepository _vigorAvistamientoReadOnlyRepository;

        public NuevoRegistro(IVigorAvistamientoReadOnlyRepository vigorAvistamientoReadOnlyRepository)
        {
            _vigorAvistamientoReadOnlyRepository = vigorAvistamientoReadOnlyRepository;
        }

        public async Task<int> Execute(int IdAvistamiento, int IdVigor, int Cantidad, string Descripcion)
        {
            var input = new Domain.MonitoreoCampoInterno.AvistamientoFlora.VigorAvistamiento();
            input.Avistamiento = new Domain.MonitoreoCampoInterno.AvistamientoFlora.AvistamientoFlora { Id = IdAvistamiento };
            input.Vigor = new Domain.MonitoreoCampoInterno.AvistamientoFlora.Vigor { Id = IdVigor };
            input.Cantidad = Cantidad;
            input.Descripcion = Descripcion;

            var lastId = await _vigorAvistamientoReadOnlyRepository.RegistrarVigorAvistamiento(input);
            return lastId;
        }
    }
}
