using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.AvistamientoFauna.LugarReferencia.GetLugaresReferencia
{
    public class GetLugaresReferencia : IGetLugaresReferencia
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public GetLugaresReferencia(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<LugaresReferenciaOutput> Execute(string filtro, int pagina, int rpp)
        {
            var input = await _gestorTablasMaestrasReadOnlyRepository.GetLugarReferencia(filtro);

            var outputs = new List<LugarReferenciaOutput>();
            foreach (var item in input)
            {
                var temp = new LugarReferenciaOutput(item);
                outputs.Add(temp);
            }
            var output = new LugaresReferenciaOutput
            {
                NLugares = input.Count,
                LugaresReferencias = outputs.Skip((pagina - 1) * rpp).Take(rpp).ToList()
            };

            return output;
        }
    }
}
