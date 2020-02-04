using GestionAmbiental.Application.Repositories.Reportes;
using GestionAmbiental.Domain.Reportes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Reportes.RAC.GetReportesTipoCondicionxNivelRiesgoSSO
{
    public class GetReportesTipoCondicionxNivelRiesgoSSO : IGetReportesTipoCondicionxNivelRiesgoSSO
    {
        private readonly IGetReportesRACReadOnlyRepository _getReportesRACReadOnlyRepository;

        public GetReportesTipoCondicionxNivelRiesgoSSO(IGetReportesRACReadOnlyRepository getReportesRACReadOnlyRepository)
        {
            _getReportesRACReadOnlyRepository = getReportesRACReadOnlyRepository ?? throw new ArgumentNullException(nameof(getReportesRACReadOnlyRepository));
        }

        public async Task<List<List<DimensionOutput>>> Execute(int IdGerencia, int IdEmpresa, int IdUsuario, string FechaInicio, string FechaFin)
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
            var data = await _getReportesRACReadOnlyRepository.GetReportesTipoCondicionxNivelRiesgoSSO(IdGerencia, IdEmpresa, IdUsuario, fInicio, fFinal);

            var output = new List<List<DimensionOutput>>();
            foreach (var item in data)
            {
                var temp = new List<DimensionOutput>();
                foreach (var subitem in item)
                {
                    temp.Add(new DimensionOutput(subitem));
                }
                temp = temp.OrderBy(x => x.Nombre).ToList();
                output.Add(temp);
            }
            return output;
        }
    }
}
