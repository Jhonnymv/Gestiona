using GestionAmbiental.Application.Repositories.DashboardGeneral;
using GestionAmbiental.Domain.Incidentes.InvestigacionFinal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.DashboardGeneral.ObtenerRegistros.Incidentes.InvestigacionFinal.ProductoDerramado
{
    public class ProductoDerramadoInvestigacion : IProductoDerramadoInvestigacion
    {
        private readonly IGetRegistrosReadOnlyRepository _getRegistrosReadOnlyRepository;

        public ProductoDerramadoInvestigacion(IGetRegistrosReadOnlyRepository getRegistrosReadOnlyRepository)
        {
            _getRegistrosReadOnlyRepository = getRegistrosReadOnlyRepository ?? throw new ArgumentNullException(nameof(getRegistrosReadOnlyRepository));
        }

        public async Task<List<Producto_Derramado_Incidente>> Execute(string startTime, string endTime)
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

            var output = await _getRegistrosReadOnlyRepository.ProductosDerramadosInvestigacion(startTimeFilter, endTimeFilter);
            output = output.OrderBy(x => x.Investigacion_Final.Codigo).ToList();
            return output;
        }
    }
}
