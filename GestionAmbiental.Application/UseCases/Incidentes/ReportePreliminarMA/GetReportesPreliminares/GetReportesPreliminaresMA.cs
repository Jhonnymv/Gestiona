using GestionAmbiental.Application.Repositories.Incidentes.ReportePreliminar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminarMA.GetReportesPreliminares
{
    public class GetReportesPreliminaresMA : IGetReportesPreliminaresMA
    {
        private readonly IGetReportePreliminarReadOnlyRepository _getReportePreliminarReadOnlyRepository;

        public GetReportesPreliminaresMA(IGetReportePreliminarReadOnlyRepository getReportePreliminarReadOnlyRepository)
        {
            _getReportePreliminarReadOnlyRepository = getReportePreliminarReadOnlyRepository ?? throw new ArgumentNullException(nameof(getReportePreliminarReadOnlyRepository));
        }

        public async Task<ReportesPreliminaresOutput> Execute(string fecha_inicio, string fecha_fin, int[] gerencia, int[] titular, string[] estado, int[] responsable, int[] asesor, string codigo, int pagina, int rpp)
        {
            DateTime? fechaInicioFiltro = null;
            DateTime? fechaFinFiltro = null;

            if (fecha_inicio != null)
                fechaInicioFiltro = Convert.ToDateTime(fecha_inicio);
            if (fecha_fin != null)
                fechaFinFiltro = Convert.ToDateTime(fecha_fin);

            var estados = new List<string>();
            if (estado.Length > 0)
            {
                if (estado.Contains("EnProceso"))
                    estados.Add("En Proceso");
                if (estado.Contains("Cerrado"))
                    estados.Add("Cerrado");
            }
            var data = await _getReportePreliminarReadOnlyRepository.GetReportesMA(fechaInicioFiltro, fechaFinFiltro, gerencia, titular, estados, responsable, asesor, codigo);

            var output = new ReportesPreliminaresOutput();
            output.Reportes = new List<ReportePreliminarOutput>();
            output.NReportes = data.Count;

            data = data.Skip((pagina - 1) * rpp).Take(rpp).ToList();
            foreach (var item in data)
            {
                var outputItem = new ReportePreliminarOutput(item);
                output.Reportes.Add(outputItem);
            }

            return output;
        }
    }
}
