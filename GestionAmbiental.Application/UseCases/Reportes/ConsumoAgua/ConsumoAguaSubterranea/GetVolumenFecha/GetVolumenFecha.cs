using GestionAmbiental.Application.Repositories.Reportes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Reportes.ConsumoAgua.ConsumoAguaSubterranea.GetVolumenFecha
{
    public class GetVolumenFecha : IGetVolumenFecha
    {
        private readonly IGetReportesConsumoAguaReadOnlyRepository _getReportesConsumoAguaReadOnlyRepository;

        public GetVolumenFecha(IGetReportesConsumoAguaReadOnlyRepository getReportesConsumoAguaReadOnlyRepository)
        {
            _getReportesConsumoAguaReadOnlyRepository = getReportesConsumoAguaReadOnlyRepository ?? throw new ArgumentNullException(nameof(getReportesConsumoAguaReadOnlyRepository));
        }

        public async Task<List<DimensionOutput>> Execute(int resolucion, string FechaInicio, string FechaFin, string frecuencia)
        {
            //Dar formato a fechas
            DateTime fInicio, fFinal;
            if (FechaInicio != null)
                fInicio = Convert.ToDateTime(FechaInicio);
            else
                fInicio = DateTime.MinValue;

            if (FechaFin != null)
                fFinal = Convert.ToDateTime(FechaFin + " 23:59:59");
            else
                fFinal = DateTime.MaxValue;
            //Obtener valores
            var data = await _getReportesConsumoAguaReadOnlyRepository.GetVolumenFecha(resolucion, 2, fInicio, fFinal, frecuencia);
            data = data.OrderBy(x => x.Index).ToList();

            var output = new List<DimensionOutput>();
            foreach (var item in data)
            {
                output.Add(new DimensionOutput(item));
            }
            return output;
        }
    }
}
