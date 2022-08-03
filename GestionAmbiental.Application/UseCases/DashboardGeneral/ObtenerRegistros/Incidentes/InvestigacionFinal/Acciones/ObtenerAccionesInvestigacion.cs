using GestionAmbiental.Application.Repositories.DashboardGeneral;
using GestionAmbiental.Domain.Incidentes.InvestigacionFinal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.DashboardGeneral.ObtenerRegistros.Incidentes.InvestigacionFinal.Acciones
{
    public class ObtenerAccionesInvestigacion : IObtenerAccionesInvestigacion
    {
        private readonly IGetRegistrosReadOnlyRepository _getRegistrosReadOnlyRepository;

        public ObtenerAccionesInvestigacion(IGetRegistrosReadOnlyRepository getRegistrosReadOnlyRepository)
        {
            _getRegistrosReadOnlyRepository = getRegistrosReadOnlyRepository ?? throw new ArgumentNullException(nameof(getRegistrosReadOnlyRepository));
        }

        public async Task<List<Accion_Incidente_Investigacion>> Execute(string startTime, string endTime)
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

            var output = await _getRegistrosReadOnlyRepository.AccionesInvestigacion(startTimeFilter, endTimeFilter);
            output = output.OrderBy(x => x.InvestigacionFinal.Codigo).ToList();
            return output;
        }
    }
}
