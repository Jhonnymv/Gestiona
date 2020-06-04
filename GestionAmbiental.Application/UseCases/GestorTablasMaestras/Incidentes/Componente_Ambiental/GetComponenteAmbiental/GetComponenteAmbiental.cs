using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Componente_Ambiental.GetComponenteAmbiental
{
    public class GetComponenteAmbiental : IGetComponenteAmbiental
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public GetComponenteAmbiental(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<ComponenteAmbientalOutput> Execute(int idComponente)
        {
            var componente = await _gestorTablasMaestrasReadOnlyRepository.GetComponente_Ambiental(idComponente);
            var output = new ComponenteAmbientalOutput(componente);
            return output;
        }
    }
}
