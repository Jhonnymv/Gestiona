using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.AvistamientoFauna.EstadoFauna.GetEstadoFauna
{
    public class GetEstadoFauna : IGetEstadoFauna
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public GetEstadoFauna(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<EstadoFaunaOutput> Execute(int idEstadoFauna)
        {
            var estadoFauna = await _gestorTablasMaestrasReadOnlyRepository.GetEstadoFauna(idEstadoFauna);
            var output = new EstadoFaunaOutput(estadoFauna);
            return output;
        }
    }
}
