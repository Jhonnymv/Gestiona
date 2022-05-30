using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.Repositories.MonitoreoCampoInterno.AvistamientoFlora.FaunaAsociadaAvistamiento;

namespace GestionAmbiental.Application.UseCases.MonitoreoCampoInterno.AvistamientoFlora.FaunaAsociadaAvistamiento.NuevoRegistro
{
    public class NuevoRegistro : INuevoRegistro
    {
        private readonly IFaunaAsociadaReadOnlyRepository _faunaAsociadaReadOnlyRepository;

        public NuevoRegistro(IFaunaAsociadaReadOnlyRepository faunaAsociadaReadOnlyRepository)
        {
            _faunaAsociadaReadOnlyRepository = faunaAsociadaReadOnlyRepository ?? throw new ArgumentNullException(nameof(faunaAsociadaReadOnlyRepository));
        }

        public async Task<int> Execute(int IdAvistamiento, int IdFaunaAsociada, int Cantidad, string Descripcion)
        {
            var input = new Domain.MonitoreoCampoInterno.AvistamientoFlora.faunaAsociadaAvistamiento();

            input.Avistamiento = new Domain.MonitoreoCampoInterno.AvistamientoFlora.AvistamientoFlora { Id = IdAvistamiento };
            input.FaunaAsociada= new Domain.MonitoreoCampoInterno.AvistamientoFlora.FaunaAsociada { Id=IdFaunaAsociada };
            input.Cantidad = Cantidad;
            input.Descripcion = Descripcion;

            var idFaunaAsociadaAvistamiento = await _faunaAsociadaReadOnlyRepository.RegistrarFaunaAsociadaAvistamiento(input);
            return idFaunaAsociadaAvistamiento;
        }
    }
}
