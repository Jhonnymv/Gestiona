using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Tipo_Incidente.GetTipoIncidente
{
    public class GetTipoIncidente : IGetTipoIncidente
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public GetTipoIncidente(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<TipoIncidenteOutput> Execute(int id)
        {
            var data = await _gestorTablasMaestrasReadOnlyRepository.GetTipo_Incidente(id);
            var output = new TipoIncidenteOutput(data);
            return output;
        }
    }
}
