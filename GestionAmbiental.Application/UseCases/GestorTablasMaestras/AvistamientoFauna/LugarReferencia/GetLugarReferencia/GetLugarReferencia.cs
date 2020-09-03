using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.AvistamientoFauna.LugarReferencia.GetLugarReferencia
{
    public class GetLugarReferencia : IGetLugarReferencia
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public GetLugarReferencia(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<LugarReferenciaOutput> Execute(int id)
        {
            var lugar = await _gestorTablasMaestrasReadOnlyRepository.GetLugarReferencia(id);
            var output = new LugarReferenciaOutput(lugar);
            return output;
        }
    }
}
