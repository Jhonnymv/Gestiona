using GestionAmbiental.Application.Repositories.DashboardGeneral;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.DashboardGeneral.ObtenerRegistros.AvistamientoFauna
{
    public class ObtenerAvistamiento : IObtenerAvistamientos
    {
        private readonly IGetRegistrosReadOnlyRepository _getRegistrosReadOnlyRepository;

        public ObtenerAvistamiento(IGetRegistrosReadOnlyRepository getRegistrosReadOnlyRepository)
        {
            _getRegistrosReadOnlyRepository = getRegistrosReadOnlyRepository ?? throw new ArgumentNullException(nameof(getRegistrosReadOnlyRepository));
        }

        public async Task<List<Domain.AvistamientoFauna.AvistamientoFauna>> Execute(string startTime, string endTime)
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

            var output = await _getRegistrosReadOnlyRepository.Avistamientos(startTimeFilter, endTimeFilter);
            output = output.OrderBy(x => x.Codigo).ToList();
            return output;
        }
    }
}
