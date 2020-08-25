using GestionAmbiental.Application.Repositories.Reportes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Reportes.AvistamientoFauna.GetNAvistamientosFauna
{
    public class GetNAvistamientoFauna : IGetNAvistamientoFauna
    {
        private readonly IGetReportesAvistamientoFaunaReadOnlyRepository _getReportesAvistamientoFaunaReadOnlyRepository;

        public GetNAvistamientoFauna(IGetReportesAvistamientoFaunaReadOnlyRepository getReportesAvistamientoFaunaReadOnlyRepository)
        {
            _getReportesAvistamientoFaunaReadOnlyRepository = getReportesAvistamientoFaunaReadOnlyRepository ?? throw new ArgumentNullException(nameof(getReportesAvistamientoFaunaReadOnlyRepository));
        }

        public async Task<List<DimensionOutput>> Execute(int nombre_comun, int tipo_avistamiento, int estado_fauna, int lugar_referencia, string FechaInicio, string FechaFin, string frecuencia)
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
            var data = await _getReportesAvistamientoFaunaReadOnlyRepository.GetNRegistrosEntreFechas(nombre_comun, tipo_avistamiento, estado_fauna, lugar_referencia, fInicio, fFinal, frecuencia);
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
