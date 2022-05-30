using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.Repositories.MonitoreoCampoInterno.AvistamientoFlora.UsoAvistamiento;

namespace GestionAmbiental.Application.UseCases.MonitoreoCampoInterno.AvistamientoFlora.UsoAvistamiento.NuevoRegistro
{
    public class NuevoRegistro: INuevoRegistro
    {
        private readonly IUsoAvistamientoReadOnlyRepository _usoAvistamientoReadOnlyRepository;

        public NuevoRegistro(IUsoAvistamientoReadOnlyRepository usoAvistamientoReadOnlyRepository)
        {
            _usoAvistamientoReadOnlyRepository = usoAvistamientoReadOnlyRepository;
        }

        public async Task<int> Execute(int IdAvistamiento, int IdUso, int Cantidad, string Descripcion)
        {
            var input = new Domain.MonitoreoCampoInterno.AvistamientoFlora.UsoAvistamiento();
            input.Avistamiento = new Domain.MonitoreoCampoInterno.AvistamientoFlora.AvistamientoFlora { Id = IdAvistamiento };
            input.Uso = new Domain.MonitoreoCampoInterno.AvistamientoFlora.Uso { Id = IdUso };
            input.Cantidad = Cantidad;
            input.Descripcion = Descripcion;

            var lastId = await _usoAvistamientoReadOnlyRepository.RegistrarUsoAvistamiento(input);
            return lastId;
        }
    }
}
