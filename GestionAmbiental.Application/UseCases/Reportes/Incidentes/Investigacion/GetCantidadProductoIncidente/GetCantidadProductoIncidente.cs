using GestionAmbiental.Application.Repositories.Reportes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Reportes.Incidentes.Investigacion.GetCantidadProductoIncidente
{
    public class GetCantidadProductoIncidente : IGetCantidadProductoIncidente
    {
        private readonly IGetReportesInvestigacionIncidenteReadOnlyRepository _getReportesInvestigacionIncidenteReadOnlyRepository;

        public GetCantidadProductoIncidente(IGetReportesInvestigacionIncidenteReadOnlyRepository getReportesInvestigacionIncidenteReadOnlyRepository)
        {
            _getReportesInvestigacionIncidenteReadOnlyRepository = getReportesInvestigacionIncidenteReadOnlyRepository ?? throw new ArgumentNullException(nameof(getReportesInvestigacionIncidenteReadOnlyRepository));
        }

        public async Task<List<List<DimensionOutput>>> Execute(int tipoIncidente, int nivelIncidente, int gerencia, int supervisor, string FechaInicio, string FechaFin)
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
            var data = await _getReportesInvestigacionIncidenteReadOnlyRepository.GetCantidadProductoIncidente(tipoIncidente, nivelIncidente, gerencia, supervisor, fInicio, fFinal);
            var output = new List<List<DimensionOutput>>();
            var outputRecuperado = new List<DimensionOutput>();
            var outputNoRecuperado = new List<DimensionOutput>();

            foreach (var item in data[1])
            {
                outputRecuperado.Add(new DimensionOutput(item));
            }
            foreach (var item in data[2])
            {
                outputNoRecuperado.Add(new DimensionOutput(item));
            }
            output.Add(outputRecuperado);
            output.Add(outputNoRecuperado);
            return output;
        }
    }
}
