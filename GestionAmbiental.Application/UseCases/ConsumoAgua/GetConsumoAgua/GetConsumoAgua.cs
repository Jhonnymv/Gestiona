using GestionAmbiental.Application.Repositories.ConsumoAgua;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.ConsumoAgua.GetConsumoAgua
{
    public class GetConsumoAgua : IGetConsumoAgua
    {
        private readonly IGetConsumoAguaReadOnlyRepository _getConsumoAguaReadOnlyRepository;

        public GetConsumoAgua(IGetConsumoAguaReadOnlyRepository getConsumoAguaReadOnlyRepository)
        {
            _getConsumoAguaReadOnlyRepository = getConsumoAguaReadOnlyRepository ?? throw new ArgumentNullException(nameof(getConsumoAguaReadOnlyRepository));
        }

        public async Task<ConsumoAguaOutput> Execute(int Id)
        {
            var data = await _getConsumoAguaReadOnlyRepository.GetConsumoAgua(Id);
            var output = new ConsumoAguaOutput(data);
            return output;
        }
    }
}
