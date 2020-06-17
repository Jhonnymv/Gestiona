using GestionAmbiental.Application.Repositories.Incidentes.ReportePreliminar;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminarMA.GetEvidenciaReportePreliminar
{
    class GetEvidenciaReportePreliminar : IGetEvidenciaReportePreliminar
    {
        private readonly IGetReportePreliminarReadOnlyRepository _getReportePreliminarReadOnlyRepository;

        public GetEvidenciaReportePreliminar(IGetReportePreliminarReadOnlyRepository getReportePreliminarReadOnlyRepository)
        {
            _getReportePreliminarReadOnlyRepository = getReportePreliminarReadOnlyRepository ?? throw new ArgumentNullException(nameof(getReportePreliminarReadOnlyRepository));
        }

        public async Task<string[]> Execute(int IdReporte)
        {
            var evidencias = await _getReportePreliminarReadOnlyRepository.GetEvidenciaReporteMA(IdReporte);
            var outputTemp = new List<string>();
            string[] output = { };
            if (evidencias.Length > 0)
            {
                var temp = JArray.Parse(evidencias);
                foreach(var item in temp)
                {
                    outputTemp.Add((string)item);
                }
                output = outputTemp.ToArray();
            }
            return output;
        }
    }
}
