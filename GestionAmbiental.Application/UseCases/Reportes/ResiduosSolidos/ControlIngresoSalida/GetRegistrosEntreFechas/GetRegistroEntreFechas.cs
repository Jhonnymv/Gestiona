using GestionAmbiental.Application.Repositories.Reportes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Reportes.ResiduosSolidos.ControlIngresoSalida.GetRegistrosEntreFechas
{
    public class GetRegistroEntreFechas : IGetRegistroEntreFechas
    {
        private readonly IGetReportesResiduosSolidosReadOnlyRepository _getReportesResiduosSolidosReadOnlyRepository;

        public GetRegistroEntreFechas(IGetReportesResiduosSolidosReadOnlyRepository getReportesResiduosSolidosReadOnlyRepository)
        {
            _getReportesResiduosSolidosReadOnlyRepository = getReportesResiduosSolidosReadOnlyRepository ?? throw new ArgumentNullException(nameof(getReportesResiduosSolidosReadOnlyRepository));
        }

        public async Task<List<List<DimensionOutput>>> Execute(int acr_cmrs, int ingreso_salida, int tipo_residuo, int motivo, string FechaInicio, string FechaFin, int gerencia, int empresa, string frecuencia)
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
            var data = await _getReportesResiduosSolidosReadOnlyRepository.GetNRegistrosEntreFechas(acr_cmrs, ingreso_salida, tipo_residuo, motivo, fInicio, fFinal,gerencia,empresa,frecuencia);            

            var output = new List<List<DimensionOutput>>();
            var ingreso = new List<DimensionOutput>();
            var salida = new List<DimensionOutput>();
            //Ingreso
            foreach (var item in data[0])
            {
                ingreso.Add(new DimensionOutput(item));
            }
            //Salida
            foreach (var item in data[1])
            {
                salida.Add(new DimensionOutput(item));
            }
            output.Add(ingreso);
            output.Add(salida);
            return output;
        }
    }
}
