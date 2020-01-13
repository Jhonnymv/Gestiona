using GestionAmbiental.Application.Repositories.RAC;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.RAC.DescargarRAC
{
    public class GetRACDescarga : IGetRACDescarga
    {
        private readonly IGetRACReadOnlyRepository _getRACReadOnlyRepository;

        public GetRACDescarga(IGetRACReadOnlyRepository getRACReadOnlyRepository)
        {
            _getRACReadOnlyRepository = getRACReadOnlyRepository ?? throw new ArgumentNullException(nameof(getRACReadOnlyRepository));
        }

        public async Task<RACOutput> Execute(int Id_RAC)
        {
            var rac = await _getRACReadOnlyRepository.GetRAC(Id_RAC);
            return new RACOutput(rac);
        }
    }
}
