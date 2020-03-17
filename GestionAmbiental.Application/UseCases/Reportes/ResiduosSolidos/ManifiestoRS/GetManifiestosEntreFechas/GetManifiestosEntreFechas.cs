using GestionAmbiental.Application.Repositories.Reportes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Reportes.ResiduosSolidos.ManifiestoRS.GetManifiestosEntreFechas
{
    public class GetManifiestosEntreFechas : IGetManifiestosEntreFechas
    {
        private readonly IGetReportesManifiestoRSReadOnlyRepository _getReportesManifiestoRSReadOnlyRepository;

        public GetManifiestosEntreFechas(IGetReportesManifiestoRSReadOnlyRepository getReportesManifiestoRSReadOnlyRepository)
        {
            _getReportesManifiestoRSReadOnlyRepository = getReportesManifiestoRSReadOnlyRepository ?? throw new ArgumentNullException(nameof(getReportesManifiestoRSReadOnlyRepository));
        }

        public async Task<List<List<DimensionOutput>>> Execute(string FechaInicio, string FechaFin, string observacion, int residuo_solido, int eors_salida, int eors_llegada, string frecuencia)
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
            var data = await _getReportesManifiestoRSReadOnlyRepository.GetNManifiestosFechas(fInicio,fFinal,observacion,residuo_solido,eors_salida,eors_llegada, frecuencia);

            var output = new List<List<DimensionOutput>>();
            var conforme = new List<DimensionOutput>();
            var anulado = new List<DimensionOutput>();
            //Ingreso
            foreach (var item in data[0])
            {
                conforme.Add(new DimensionOutput(item));
            }
            //Salida
            foreach (var item in data[1])
            {
                anulado.Add(new DimensionOutput(item));
            }
            output.Add(conforme);
            output.Add(anulado);
            return output;
        }
    }
}
