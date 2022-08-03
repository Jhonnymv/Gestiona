using GestionAmbiental.Application.Repositories.DashboardGeneral;
using GestionAmbiental.Domain.Inspecciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.DashboardGeneral.ObtenerRegistros.Inspecciones
{
    public class ObtenerRegistrosInspecciones : IObtenerRegistrosInspecciones
    {
        private readonly IGetRegistrosReadOnlyRepository _getRegistrosReadOnlyRepository;

        public ObtenerRegistrosInspecciones(IGetRegistrosReadOnlyRepository getRegistrosReadOnlyRepository)
        {
            _getRegistrosReadOnlyRepository = getRegistrosReadOnlyRepository ?? throw new ArgumentNullException(nameof(getRegistrosReadOnlyRepository));
        }

        public async Task<List<InspeccionDetalle>> Execute(string startTime, string endTime)
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

            var output = await _getRegistrosReadOnlyRepository.Observaciones(startTimeFilter, endTimeFilter);
            output = output.OrderBy(x => x.Cabecera.Codigo).ToList();
            return output;
        }
    }
}
