using GestionAmbiental.Application.Repositories.Reportes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Reportes.ConsumoAgua.ConsumoAguaSubterranea.GetVolumenRestante
{
    public class GetVolumenRestante : IGetVolumenRestante
    {
        private readonly IGetReportesConsumoAguaReadOnlyRepository _getReportesConsumoAguaReadOnlyRepository;

        public GetVolumenRestante(IGetReportesConsumoAguaReadOnlyRepository getReportesConsumoAguaReadOnlyRepository)
        {
            _getReportesConsumoAguaReadOnlyRepository = getReportesConsumoAguaReadOnlyRepository ?? throw new ArgumentNullException(nameof(getReportesConsumoAguaReadOnlyRepository));
        }

        public async Task<List<List<DimensionOutput>>> Execute(int resolucion)
        {

            //Obtener valores
            var data = await _getReportesConsumoAguaReadOnlyRepository.GetVolumenRestante(resolucion, 2);
            //data = data.OrderBy(x => x.Index).ToList();

            var output = new List<List<DimensionOutput>>();
            var usado = new List<DimensionOutput>();
            var porUsar = new List<DimensionOutput>();

            foreach (var item in data[0])
                usado.Add(new DimensionOutput(item));
            foreach (var item in data[1])
                porUsar.Add(new DimensionOutput(item));

            output.Add(usado);
            output.Add(porUsar);

            return output;
        }
    }
}
