using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.AvistamientoFauna.TipoFauna.GetTipoFauna
{
    public class GetTipoFauna : IGetTipoFauna
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public GetTipoFauna(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<TipoFaunaOutput> Execute(int idTipoFauna)
        {
            var tipoFauna = await _gestorTablasMaestrasReadOnlyRepository.GetTipoFauna(idTipoFauna);
            var output = new TipoFaunaOutput(tipoFauna);
            return output;
        }
    }
}
