using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.Repositories.MonitoreoCampoInterno.AvistamientoFlora.FenologiaAvistamiento;

namespace GestionAmbiental.Application.UseCases.MonitoreoCampoInterno.AvistamientoFlora.FenologiaAvistamiento.NuevoRegistro
{
    public class NuevoRegistro: INuevoRegistro
    {
        private readonly IFenologiaAvistamientoReadOnlyRepository _fenologiaAvistamientoReadOnlyRepository;

        public NuevoRegistro(IFenologiaAvistamientoReadOnlyRepository fenologiaAvistamientoReadOnlyRepository)
        {
            _fenologiaAvistamientoReadOnlyRepository = fenologiaAvistamientoReadOnlyRepository;
        }

        public async Task<int> Execute(int IdAvistamiento, int IdFenologia, int Cantidad, string Descripcion)
        {
            var input = new Domain.MonitoreoCampoInterno.AvistamientoFlora.FenologiaAvistamiento();
            input.Avistamiento = new Domain.MonitoreoCampoInterno.AvistamientoFlora.AvistamientoFlora { Id = IdAvistamiento };
            input.Fenologia = new Domain.MonitoreoCampoInterno.AvistamientoFlora.Fenologia { Id = IdFenologia };
            input.Cantidad = Cantidad;
            input.Descripcion = Descripcion;

            var idFenologiaAvistamiento = await _fenologiaAvistamientoReadOnlyRepository.RegistrarFenologiaAsociadaAvistamiento(input);
            return idFenologiaAvistamiento;
        }
    }
}
