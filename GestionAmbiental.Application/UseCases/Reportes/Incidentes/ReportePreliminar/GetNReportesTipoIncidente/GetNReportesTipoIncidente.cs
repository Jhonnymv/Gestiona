using GestionAmbiental.Application.Repositories.Reportes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Reportes.Incidentes.ReportePreliminar.GetNReportesTipoIncidente
{
    public class GetNReportesTipoIncidente : IGetNReportesTipoIncidente
    {
        private readonly IGetReportesReportePreliminarIncidenteReadOnlyRepository _getReportesReportePreliminarIncidenteReadOnlyRepository;

        public GetNReportesTipoIncidente(IGetReportesReportePreliminarIncidenteReadOnlyRepository getReportesReportePreliminarIncidenteReadOnlyRepository)
        {
            _getReportesReportePreliminarIncidenteReadOnlyRepository = getReportesReportePreliminarIncidenteReadOnlyRepository ?? throw new ArgumentNullException(nameof(getReportesReportePreliminarIncidenteReadOnlyRepository));
        }

        public async Task<List<DimensionOutput>> Execute(int tipoIncidente, int nivelIncidente, int gerencia, int supervisor, string FechaInicio, string FechaFin)
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
            var data = await _getReportesReportePreliminarIncidenteReadOnlyRepository.GetNReportesTipoIncidente(tipoIncidente, nivelIncidente, gerencia, supervisor, fInicio, fFinal);
            data = data.OrderByDescending(x => x.Valor).ToList();

            var output = new List<DimensionOutput>();
            foreach (var item in data)
            {
                output.Add(new DimensionOutput(item));
            }
            return output;
        }
    }
}
