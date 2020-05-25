using GestionAmbiental.Application.Repositories.Reportes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Reportes.Incidentes.Investigacion.GetImpactoSuelo
{
    public class GetImpactoSuelo : IGetImpactoSuelo
    {
        private readonly IGetReportesInvestigacionIncidenteReadOnlyRepository _getReportesInvestigacionIncidenteReadOnlyRepository;

        public GetImpactoSuelo(IGetReportesInvestigacionIncidenteReadOnlyRepository getReportesInvestigacionIncidenteReadOnlyRepository)
        {
            _getReportesInvestigacionIncidenteReadOnlyRepository = getReportesInvestigacionIncidenteReadOnlyRepository ?? throw new ArgumentNullException(nameof(getReportesInvestigacionIncidenteReadOnlyRepository));
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
            var data = await _getReportesInvestigacionIncidenteReadOnlyRepository.GetImpactoSuelo(tipoIncidente, nivelIncidente, gerencia, supervisor, fInicio, fFinal);

            var output = new List<DimensionOutput>();
            foreach (var item in data)
            {
                output.Add(new DimensionOutput(item));
            }
            return output;
        }
    }
}
