using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.Repositories.CompromisosMCB.AreaCMCB;

namespace GestionAmbiental.Application.UseCases.CompromisosMCB.AreaCMCB.EditarRegistro
{
    public class EditarRegistro : IEditarRegistro
    {
        private readonly IAreaReadOnlyRepository _areaReadOnlyRepository;

        public EditarRegistro(IAreaReadOnlyRepository areaReadOnlyRepository)
        {
            _areaReadOnlyRepository = areaReadOnlyRepository ?? throw new ArgumentNullException(nameof(areaReadOnlyRepository));
        }

        public async Task Execute(int Id, string Area)
        {
            var input = new Domain.CompromisosMCB.AreaCMCB();
            input.Id = Id;
            input.Area = Area;

            await _areaReadOnlyRepository.EditarArea(input);

        }
    }
}
