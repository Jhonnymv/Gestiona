using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.Repositories.MonitoreoCampoInterno.AvistamientoFlora.NombreCientificoAvistamiento;

namespace GestionAmbiental.Application.UseCases.MonitoreoCampoInterno.AvistamientoFlora.NombreCientificoAvistamiento.NuevoRegistro
{
    public class NuevoRegistro: INuevoRegistro
    {
        private readonly INombreCientificoAvistamientoReadOnlyRepository _nombreCientificoAvistamientoReadOnlyRepository;

        public NuevoRegistro(INombreCientificoAvistamientoReadOnlyRepository nombreCientificoAvistamientoReadOnlyRepository)
        {
            _nombreCientificoAvistamientoReadOnlyRepository = nombreCientificoAvistamientoReadOnlyRepository;
        }

        public async Task<int> Execute(int IdAvistamiento, int IdNombreCientifico, int Cantidad, string Descripcion)
        {
            var input = new Domain.MonitoreoCampoInterno.AvistamientoFlora.NombreCientificoAvistamiento();
            input.Avistamiento = new Domain.MonitoreoCampoInterno.AvistamientoFlora.AvistamientoFlora { Id = IdAvistamiento };
            input.NombreCientifico = new Domain.MonitoreoCampoInterno.AvistamientoFlora.NombreCientifico { Id = IdNombreCientifico };
            input.Cantidad = Cantidad;
            input.Descripcion = Descripcion;

            var lastId = await _nombreCientificoAvistamientoReadOnlyRepository.RegistrarNombreCientificoaAvistamiento(input);
            return lastId;
        }
    }
}
