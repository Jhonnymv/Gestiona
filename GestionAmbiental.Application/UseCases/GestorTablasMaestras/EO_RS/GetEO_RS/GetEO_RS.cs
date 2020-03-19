using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.EO_RS.GetEO_RS
{
    public class GetEO_RS : IGetEO_RS
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public GetEO_RS(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<EO_RSOutput> Execute(int idEORS)
        {
            var eors = await _gestorTablasMaestrasReadOnlyRepository.GetEO_RS(idEORS);
            var output = new EO_RSOutput(eors);
            return output;
        }
    }
}
