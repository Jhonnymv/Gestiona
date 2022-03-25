using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.Repositories.CompromisosMCB.AreaCMCB;

namespace GestionAmbiental.Application.UseCases.CompromisosMCB.AreaCMCB.NuevoRegistro
{
    public class NuevoRegistro:INuevoRegistro
    {
        private readonly IAreaReadOnlyRepository _areaReadOnlyRepository;

        public NuevoRegistro(IAreaReadOnlyRepository areaReadOnlyRepository)
        {
            _areaReadOnlyRepository = areaReadOnlyRepository ?? throw new ArgumentNullException(nameof(areaReadOnlyRepository));
        }
        public async Task<int> Execute(string Area)
        {
            var input = new Domain.CompromisosMCB.AreaCMCB
            {
                Area = Area
            };
            return await _areaReadOnlyRepository.RegistrarArea(input);
        }
    }
}
