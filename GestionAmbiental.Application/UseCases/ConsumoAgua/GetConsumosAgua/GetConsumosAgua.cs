using GestionAmbiental.Application.Repositories.ConsumoAgua;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.ConsumoAgua.GetConsumosAgua
{
    public class GetConsumosAgua : IGetConsumosAgua
    {
        private readonly IGetConsumoAguaReadOnlyRepository _getConsumoAguaReadOnlyRepository;

        public GetConsumosAgua(IGetConsumoAguaReadOnlyRepository getConsumoAguaReadOnlyRepository)
        {
            _getConsumoAguaReadOnlyRepository = getConsumoAguaReadOnlyRepository ?? throw new ArgumentNullException(nameof(getConsumoAguaReadOnlyRepository));
        }

        public async Task<ConsumosAguaOutput> Execute(string fecha, int pagina, int rpp,int origen_agua)
        {
            DateTime? fechaFiltro = null;
            if (fecha != null)
                fechaFiltro = Convert.ToDateTime(fecha);

            var data = await _getConsumoAguaReadOnlyRepository.GetConsumosAgua(fechaFiltro);
            data = data.Where(x=>x.ResolucionConsumoAgua.FuenteAgua.OrigenAgua.IdOrigenAgua.Equals(origen_agua)).OrderByDescending(x => x.LecturaFinal).ToList();

            var output = new ConsumosAguaOutput();
            output.Consumos = new List<ConsumoAguaOutput>();
            output.NConsumo = data.Count;
            data = data.Skip((pagina - 1) * rpp).Take(rpp).ToList();

            foreach (var item in data)
            {
                output.Consumos.Add(new ConsumoAguaOutput(item));
            }
            return output;
        }

        public async Task<ConsumosAguaOutput> Execute(string fecha, int origen_agua)
        {
            DateTime fechaFiltro = Convert.ToDateTime(fecha);

            var data = await _getConsumoAguaReadOnlyRepository.GetConsumosAguaMensual(fechaFiltro);
            data = data.Where(x => x.ResolucionConsumoAgua.FuenteAgua.OrigenAgua.IdOrigenAgua.Equals(origen_agua)).OrderByDescending(x => x.LecturaFinal).ToList();

            var output = new ConsumosAguaOutput();
            output.Consumos = new List<ConsumoAguaOutput>();
            output.NConsumo = data.Count;

            foreach (var item in data)
            {
                output.Consumos.Add(new ConsumoAguaOutput(item));
            }
            return output;
        }
    }
}
