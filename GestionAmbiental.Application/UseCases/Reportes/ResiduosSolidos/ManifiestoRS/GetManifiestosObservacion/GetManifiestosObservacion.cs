using GestionAmbiental.Application.Repositories.Reportes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Reportes.ResiduosSolidos.ManifiestoRS.GetManifiestosObservacion
{
    public class GetManifiestosObservacion : IGetManifiestosObservacion
    {
        private readonly IGetReportesManifiestoRSReadOnlyRepository _getReportesManifiestoRSReadOnlyRepository;

        public GetManifiestosObservacion(IGetReportesManifiestoRSReadOnlyRepository getReportesManifiestoRSReadOnlyRepository)
        {
            _getReportesManifiestoRSReadOnlyRepository = getReportesManifiestoRSReadOnlyRepository ?? throw new ArgumentNullException(nameof(getReportesManifiestoRSReadOnlyRepository));
        }

        public async Task<List<DimensionOutput>> Execute(string FechaInicio, string FechaFin, string observacion, int residuo_solido, int eors_salida, int eors_llegada)
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
            var data = await _getReportesManifiestoRSReadOnlyRepository.GetNManifiestosObservacion(fInicio,fFinal,observacion,residuo_solido,eors_salida,eors_llegada);

            var output = new List<DimensionOutput>();
            foreach (var item in data)
            {
                output.Add(new DimensionOutput(item));
            }
            return output;
        }
    }
}
