using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.ConsumoAgua.OrigenAgua.GetOrigenAgua
{
    public class GetOrigenAgua : IGetOrigenAgua
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public GetOrigenAgua(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<OrigenAguaOutput> Execute(int id)
        {
            var data = await _gestorTablasMaestrasReadOnlyRepository.GetOrigenAgua(id);
            var output = new OrigenAguaOutput(data);
            return output;
        }
    }
}
