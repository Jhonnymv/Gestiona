using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.TipoCondicion.GetTipoCondicion
{
    public class GetTipoCondicion : IGetTipoCondicion
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public GetTipoCondicion(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<TipoCondicionOutput> Execute(int idTipoCondicion)
        {
            var tipoCondicion = await _gestorTablasMaestrasReadOnlyRepository.GetTipoCondicion(idTipoCondicion);
            var output = new TipoCondicionOutput(tipoCondicion);
            return output;
        }
    }
}
