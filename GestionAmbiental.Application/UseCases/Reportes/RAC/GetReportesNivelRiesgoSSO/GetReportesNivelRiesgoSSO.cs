using GestionAmbiental.Application.Repositories.Reportes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Reportes.RAC.GetReportesNivelRiesgoSSO
{
    public class GetReportesNivelRiesgoSSO : IGetReportesNivelRiesgoSSO
    {
        private readonly IGetReportesRACReadOnlyRepository _getReportesRACReadOnlyRepository;
        public GetReportesNivelRiesgoSSO(IGetReportesRACReadOnlyRepository getReportesRACReadOnlyRepository)
        {
            _getReportesRACReadOnlyRepository = getReportesRACReadOnlyRepository ?? throw new ArgumentNullException(nameof(getReportesRACReadOnlyRepository));
        }
        public async Task<List<DimensionOutput>> Execute(int IdGerencia, int IdEmpresa, int IdUsuario, string FechaInicio, string FechaFin)
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
            var data = await _getReportesRACReadOnlyRepository.GetReportesNivelRiesgoSSO(IdGerencia, IdEmpresa, IdUsuario, fInicio, fFinal);

            var output = new List<DimensionOutput>();
            foreach (var item in data)
            {
                output.Add(new DimensionOutput(item));
            }
            return output;
        }
    }
}
