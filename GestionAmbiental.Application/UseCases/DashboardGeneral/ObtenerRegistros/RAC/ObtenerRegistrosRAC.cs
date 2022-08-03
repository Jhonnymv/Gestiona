using GestionAmbiental.Application.Repositories.DashboardGeneral;
using GestionAmbiental.Domain.Inspecciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.DashboardGeneral.ObtenerRegistros.RAC
{
    public class ObtenerRegistrosRAC : IObtenerRegistrosRAC
    {
        private readonly IGetRegistrosReadOnlyRepository _getRegistrosReadOnlyRepository;

        public ObtenerRegistrosRAC(IGetRegistrosReadOnlyRepository getRegistrosReadOnlyRepository)
        {
            _getRegistrosReadOnlyRepository = getRegistrosReadOnlyRepository ?? throw new ArgumentNullException(nameof(getRegistrosReadOnlyRepository));
        }

        public async Task<List<Domain.RAC.RAC>> Execute(string startTime, string endTime)
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

            var output = await _getRegistrosReadOnlyRepository.RACs(startTimeFilter, endTimeFilter);
            output = output.OrderBy(x => x.Codigo).ToList();
            return output;
        }
    }
}
