using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Factor_Causa_Incidente.GetFactorCausa
{
    public class GetFactorCausa : IGetFactorCausa
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public GetFactorCausa(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<FactorCausaOutput> Execute(int id)
        {
            var data = await _gestorTablasMaestrasReadOnlyRepository.GetFactor_Causa_Incidente(id);
            var output = new FactorCausaOutput(data);
            return output;
        }
    }
}
