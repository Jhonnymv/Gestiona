using GestionAmbiental.Application.Repositories.Incidentes.ReportePreliminar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.GetReportesPreliminares
{
    public class GetReportesPreliminares : IGetReportesPreliminares
    {
        private readonly IGetReportePreliminarReadOnlyRepository _getReportePreliminarReadOnlyRepository;

        public GetReportesPreliminares(IGetReportePreliminarReadOnlyRepository getReportePreliminarReadOnlyRepository)
        {
            _getReportePreliminarReadOnlyRepository = getReportePreliminarReadOnlyRepository ?? throw new ArgumentNullException(nameof(getReportePreliminarReadOnlyRepository));
        }
        public async Task<ReportesPreliminaresOutput> Execute(string fecha, int id_Usuario, int id_Gerencia,int id_Asesor, int pagina, int rpp)
        {
            DateTime? fechaFiltro = null;
            if (fecha != null)
                fechaFiltro = Convert.ToDateTime(fecha);
            var data = await _getReportePreliminarReadOnlyRepository.GetReportes(fechaFiltro,id_Asesor,id_Usuario,id_Gerencia);

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
