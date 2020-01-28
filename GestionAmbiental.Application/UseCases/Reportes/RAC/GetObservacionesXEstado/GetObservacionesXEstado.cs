using GestionAmbiental.Application.Repositories.Reportes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Reportes.RAC.GetObservacionesXEstado
{
    public class GetObservacionesXEstado : IGetObservacionesXEstado
    {
        private readonly IGetReportesRACReadOnlyRepository _getReportesRACReadOnlyRepository;

        public GetObservacionesXEstado(IGetReportesRACReadOnlyRepository getReportesRACReadOnlyRepository)
        {
            _getReportesRACReadOnlyRepository = getReportesRACReadOnlyRepository ?? throw new ArgumentNullException(nameof(getReportesRACReadOnlyRepository));
        }

        public async Task<List<DimensionOutput>> Execute(int IdGerencia, int IdEmpresa, int IdUsuario)
        {
            var data = await _getReportesRACReadOnlyRepository.GetObservacionesXEstado(IdGerencia,IdEmpresa,IdUsuario);
            var output = new List<DimensionOutput>();
            foreach (var item in data)
            {
                output.Add(new DimensionOutput(item));
            }
            return output;
        }
    }
}
