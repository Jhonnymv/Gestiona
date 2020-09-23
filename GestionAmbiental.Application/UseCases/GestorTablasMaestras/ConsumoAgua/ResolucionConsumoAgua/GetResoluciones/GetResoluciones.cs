using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.ConsumoAgua.ResolucionConsumoAgua.GetResoluciones
{
    public class GetResoluciones : IGetResoluciones
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public GetResoluciones(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<ResolucionesConsumoAguaOutput> Execute(string filtro, int pagina, int rpp)
        {
            var input = await _gestorTablasMaestrasReadOnlyRepository.GetResolucionConsumoAgua(filtro);

            var outputs = new List<ResolucionConsumoAguaOutput>();
            foreach (var item in input)
            {
                var temp = new ResolucionConsumoAguaOutput(item);
                outputs.Add(temp);
            }
            var output = new ResolucionesConsumoAguaOutput
            {
                NResoluciones = input.Count,
                Resoluciones = outputs.Skip((pagina - 1) * rpp).Take(rpp).ToList()
            };

            return output;
        }
    }
}
