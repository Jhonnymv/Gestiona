using GestionAmbiental.Application.Repositories.AvistamientoFauna;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.AvistamienoFauna.GetAvistamientoFauna
{
    public class GetAvistamientoFauna : IGetAvistamientoFauna
    {
        private readonly IGetAvistamientoFaunaReadOnlyRepository _getAvistamientoFaunaReadOnlyRepository;

        public GetAvistamientoFauna(IGetAvistamientoFaunaReadOnlyRepository getAvistamientoFaunaReadOnlyRepository)
        {
            _getAvistamientoFaunaReadOnlyRepository = getAvistamientoFaunaReadOnlyRepository ?? throw new ArgumentNullException(nameof(getAvistamientoFaunaReadOnlyRepository));
        }

        public async Task<AvistamientoFaunaOutput> Execute(int IdAvistamiento)
        {
            var avistamiento = await _getAvistamientoFaunaReadOnlyRepository.GetAvistamientoFauna(IdAvistamiento);
            var output = new AvistamientoFaunaOutput(avistamiento);
            return output;

        }
    }
}
