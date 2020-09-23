using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.ConsumoAgua.FuenteAgua.GetFuentesAgua
{
    public class GetFuentesAgua : IGetFuentesAgua
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public GetFuentesAgua(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<FuentesAguaOutput> Execute(string filtro, int pagina, int rpp)
        {
            var input = await _gestorTablasMaestrasReadOnlyRepository.GetFuenteAgua(filtro);

            var outputs = new List<FuenteAguaOutput>();
            foreach (var item in input)
            {
                var temp = new FuenteAguaOutput(item);
                outputs.Add(temp);
            }
            var output = new FuentesAguaOutput
            {
                NFuentes = input.Count,
                Fuentes = outputs.Skip((pagina - 1) * rpp).Take(rpp).ToList()
            };

            return output;
        }

        public async Task<FuentesAguaOutput> Execute()
        {
            var input = await _gestorTablasMaestrasReadOnlyRepository.GetFuenteAgua(null);

            var outputs = new List<FuenteAguaOutput>();
            foreach (var item in input)
            {
                var temp = new FuenteAguaOutput(item);
                outputs.Add(temp);
            }
            var output = new FuentesAguaOutput
            {
                NFuentes = input.Count,
                Fuentes = outputs
            };

            return output;
        }
    }
}
