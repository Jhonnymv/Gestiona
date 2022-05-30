using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.Repositories.MonitoreoCampoInterno.AvistamientoFlora.SaludAvistamiento;

namespace GestionAmbiental.Application.UseCases.MonitoreoCampoInterno.AvistamientoFlora.SaludAvistamiento.NuevoRegistro
{
    public class NuevoRegistro: INuevoRegistro
    {
        private readonly ISaludAvistamientoReadOnlyRepository _saludAvistamientoReadOnlyRepository;

        public NuevoRegistro(ISaludAvistamientoReadOnlyRepository saludAvistamientoReadOnlyRepository)
        {
            _saludAvistamientoReadOnlyRepository = saludAvistamientoReadOnlyRepository;
        }

        public async Task<int> Execute(int IdAvistamiento, int IdSalud, int Cantidad, string Descripcion)
        {
            var input = new Domain.MonitoreoCampoInterno.AvistamientoFlora.SaludAvistamiento();
            input.Avistamiento = new Domain.MonitoreoCampoInterno.AvistamientoFlora.AvistamientoFlora { Id = IdAvistamiento };
            input.Salud = new Domain.MonitoreoCampoInterno.AvistamientoFlora.Salud { Id = IdSalud };
            input.Cantidad = Cantidad;
            input.Descripcion = Descripcion;

            var lastId = await _saludAvistamientoReadOnlyRepository.RegistrarSaludAvistamiento(input);
            return lastId;
        }
    }
}
