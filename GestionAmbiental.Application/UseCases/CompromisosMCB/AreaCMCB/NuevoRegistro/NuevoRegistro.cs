using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.Repositories.CompromisosMCB.AreaCMCB;
using GestionAmbiental.Application.Repositories.CompromisosMCB.CompromisoCMCB;

namespace GestionAmbiental.Application.UseCases.CompromisosMCB.AreaCMCB.NuevoRegistro
{
    public class NuevoRegistro : INuevoRegistro
    {
        private readonly IAreaReadOnlyRepository _areaReadOnlyRepository;
        private readonly IGetItemsReadOnlyRepository _getItemsReadOnlyRepository;

        public NuevoRegistro(IAreaReadOnlyRepository areaReadOnlyRepository, IGetItemsReadOnlyRepository getItemsReadOnlyRepository)
        {
            _areaReadOnlyRepository = areaReadOnlyRepository;
            _getItemsReadOnlyRepository = getItemsReadOnlyRepository;
        }

        public async Task<int> Execute(string Area)
        {
            var existentes = await _getItemsReadOnlyRepository.GetAreas();

            var existe = existentes.Find(x => x.Area.Equals(Area));

            if (existe != null)
            {
                return existe.Id;
            }
            else
            {
                var input = new Domain.CompromisosMCB.AreaCMCB
                {
                    Area = Area
                };
                return await _areaReadOnlyRepository.RegistrarArea(input);
            }
        }
    }
}
