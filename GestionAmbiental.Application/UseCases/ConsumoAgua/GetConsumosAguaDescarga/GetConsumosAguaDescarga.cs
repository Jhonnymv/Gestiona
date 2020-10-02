using GestionAmbiental.Application.Repositories.ConsumoAgua;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.ConsumoAgua.GetConsumosAguaDescarga
{
    public class GetConsumosAguaDescarga : IGetConsumosAguaDescarga
    {
        private readonly IGetConsumoAguaReadOnlyRepository _getConsumoAguaReadOnlyRepository;

        public GetConsumosAguaDescarga(IGetConsumoAguaReadOnlyRepository getConsumoAguaReadOnlyRepository)
        {
            _getConsumoAguaReadOnlyRepository = getConsumoAguaReadOnlyRepository ?? throw new ArgumentNullException(nameof(getConsumoAguaReadOnlyRepository));
        }

        public async Task<List<ConsumoAguaOutput>> ExecuteFechas(string fechaInicio, string fechaFin, int resolucion)
        {
            var FechaInicio = DateTime.Parse(fechaInicio);
            var FechaFin = DateTime.Parse(fechaFin);
            var consumos = await _getConsumoAguaReadOnlyRepository.GetConsumosAguaDescarga(FechaInicio, FechaFin, resolucion);
            consumos = consumos.OrderBy(x => x.Fecha).ToList();

            var output = new List<ConsumoAguaOutput>();
            foreach (var item in consumos)
            {
                output.Add(new ConsumoAguaOutput(item));
            }
            return output;
        }

        public async Task<List<ConsumoMensualOutput>> ExecuteMensual(int resolucion)
        {
            var consumos = await _getConsumoAguaReadOnlyRepository.GetConsumosAguaDescarga(resolucion);
            
            var mensual= consumos.GroupBy(x =>x.Fecha.Month).OrderBy(x => x.Key).ToList();

            var output = new List<ConsumoMensualOutput>();
            //Calcular TiempoOperacion, VolumenMensual y Caudal
            for (int i = 0;i<mensual.Count;i++)
            {
                var TiempoOperacion = Math.Round(mensual[i].Sum(x => x.TiempoBombeo), 2);
                var VolumenMensual = Math.Round(mensual[i].Sum(x => x.VolumenBombeado), 2);
                var Caudal = Math.Round(VolumenMensual/(TiempoOperacion*3600), 2);

                //var VolumenAcumulado = Math.Round(i > 0 ? VolumenMensual : VolumenMensual + mensual[i - 1].Sum(x => x.VolumenBombeado), 2);


                var outputItem = new ConsumoMensualOutput
                {
                    Mes = mensual[i].Key,
                    TiempoOperacion = TiempoOperacion,
                    VolumenMensual = VolumenMensual,
                    Caudal = Caudal
                };
                output.Add(outputItem);
            }

            //Calcular VolumenAcumulado
            for (int i = 0; i < output.Count; i++)
            {
                var VolumenAcumulado = Math.Round(i > 0 ? output[i].VolumenMensual + output[i - 1].VolumenAcumulado : output[i].VolumenMensual, 2);
                output[i].VolumenAcumulado = VolumenAcumulado;
            }

            return output;
        }
    }
}
