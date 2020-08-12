using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.AreaTrabajo.GetAreaTrabajo
{
    public class GetAreaTrabajo : IGetAreaTrabajo
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public GetAreaTrabajo(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<AreaTrabajoOutput> Execute(int idEstadoFauna)
        {
            var estadoFauna = await _gestorTablasMaestrasReadOnlyRepository.GetAreaTrabajo(idEstadoFauna);
            var output = new AreaTrabajoOutput(estadoFauna);
            return output;
        }
    }
}
