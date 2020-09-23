using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.ConsumoAgua.OrigenAgua.GetOrigenesAgua
{
    public class GetOrigenesAgua : IGetOrigenesAgua
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public GetOrigenesAgua(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<OrigenesAguaOutput> Execute(string filtro, int pagina, int rpp)
        {
            var input = await _gestorTablasMaestrasReadOnlyRepository.GetOrigenAgua(filtro);

            var outputs = new List<OrigenAguaOutput>();
            foreach (var item in input)
            {
                var temp = new OrigenAguaOutput(item);
                outputs.Add(temp);
            }
            var output = new OrigenesAguaOutput
            {
                NOrigenes = input.Count,
                Origenes = outputs.Skip((pagina - 1) * rpp).Take(rpp).ToList()
            };

            return output;
        }

        public async Task<OrigenesAguaOutput> Execute()
        {
            var input = await _gestorTablasMaestrasReadOnlyRepository.GetOrigenAgua(null);
            var outputs = new List<OrigenAguaOutput>();
            foreach (var item in input)
            {
                var temp = new OrigenAguaOutput(item);
                outputs.Add(temp);
            }
            var output = new OrigenesAguaOutput
            {
                NOrigenes = input.Count,
                Origenes = outputs
            };

            return output;
        }
    }
}
