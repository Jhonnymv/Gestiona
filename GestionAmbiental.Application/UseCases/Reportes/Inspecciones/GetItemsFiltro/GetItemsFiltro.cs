using GestionAmbiental.Application.Repositories.Reportes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Reportes.Inspecciones.GetItemsFiltro
{
    public class GetItemsFiltro : IGetItemsFiltro
    {
        private readonly IGetItemsFiltroReadOnlyRepository _filtroReadOnlyRepository;
        public GetItemsFiltro(IGetItemsFiltroReadOnlyRepository filtroReadOnlyRepository)
        {
            _filtroReadOnlyRepository = filtroReadOnlyRepository ?? throw new ArgumentNullException(nameof(filtroReadOnlyRepository));
        }
        public async Task<ItemsFiltroOutput> Execute()
        {
            var output = new ItemsFiltroOutput();

            output.AsesoresMAP = await _filtroReadOnlyRepository.GetAsesoresMA();
            output.AsesoresSSO = await _filtroReadOnlyRepository.GetAsesorSSO();
            output.RespLevObs = await _filtroReadOnlyRepository.GetRespLevObs();
            output.Empresas = await _filtroReadOnlyRepository.GetEmpresas();
            output.Gerencias = await _filtroReadOnlyRepository.GetGerencias();

            return output;
        }
    }
}
