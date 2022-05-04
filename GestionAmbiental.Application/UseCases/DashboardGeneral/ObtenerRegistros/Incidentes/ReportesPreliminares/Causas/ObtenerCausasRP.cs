using GestionAmbiental.Application.Repositories.DashboardGeneral;
using GestionAmbiental.Domain.Incidentes.ReportePreliminar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.DashboardGeneral.ObtenerRegistros.Incidentes.ReportesPreliminares.Causas
{
    public class ObtenerCausasRP : IObtenerCausasRP
    {
        private readonly IGetRegistrosReadOnlyRepository _getRegistrosReadOnlyRepository;
        public ObtenerCausasRP(IGetRegistrosReadOnlyRepository getRegistrosReadOnlyRepository)
        {
            _getRegistrosReadOnlyRepository = getRegistrosReadOnlyRepository ?? throw new ArgumentNullException(nameof(getRegistrosReadOnlyRepository));
        }
        public async Task<List<Causa_Incidente_Reporte>> Execute(string startTime, string endTime)
        {
            DateTime startTimeFilter, endTimeFilter;

            if (startTime != null)
                startTimeFilter = Convert.ToDateTime(startTime);
            else
                startTimeFilter = DateTime.MinValue;

            if (endTime != null)
                endTimeFilter = Convert.ToDateTime(endTime);
            else
                endTimeFilter = DateTime.MaxValue;

            var output = await _getRegistrosReadOnlyRepository.CausasReportePreliminar(startTimeFilter, endTimeFilter);
            output = output.OrderBy(x => x.Reporte.Codigo).ToList();
            return output;
        }
    }
}
