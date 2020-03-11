using GestionAmbiental.Application.Repositories.Reportes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Reportes.ResiduosSolidos.ControlIngresoSalida.GetRegistrosIngresoSalida
{
    public class GetRegistrosIngresoSalida : IGetRegistrosIngresoSalida
    {
        private readonly IGetReportesResiduosSolidosReadOnlyRepository _getReportesResiduosSolidosReadOnlyRepository;

        public GetRegistrosIngresoSalida(IGetReportesResiduosSolidosReadOnlyRepository getReportesResiduosSolidosReadOnlyRepository)
        {
            _getReportesResiduosSolidosReadOnlyRepository = getReportesResiduosSolidosReadOnlyRepository ?? throw new ArgumentNullException(nameof(getReportesResiduosSolidosReadOnlyRepository));
        }

        public async Task<List<DimensionOutput>> Execute(int acr_cmrs, int ingreso_salida, int tipo_residuo, int motivo, string FechaInicio, string FechaFin, int responsable, int empresa)
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
            var data = await _getReportesResiduosSolidosReadOnlyRepository.GetNRegistrosIngresoSalida(acr_cmrs, ingreso_salida, tipo_residuo, motivo, fInicio, fFinal, responsable, empresa);

            var output = new List<DimensionOutput>();
            foreach (var item in data)
            {
                output.Add(new DimensionOutput(item));
            }
            return output;
        }
    }
}
