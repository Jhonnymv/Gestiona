using GestionAmbiental.Application.Repositories.RAC;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Notificaciones.ImplementacionRAC
{
    public class GetImplementacionRACInfo : IGetImplementacionRACInfo
    {
        private readonly IGetRACReadOnlyRepository _getRACReadOnlyRepository;
        public GetImplementacionRACInfo(IGetRACReadOnlyRepository getRACReadOnlyRepository)
        {
            _getRACReadOnlyRepository = getRACReadOnlyRepository ?? throw new ArgumentNullException(nameof(getRACReadOnlyRepository));
        }

        public async Task<ImplementacionRACOutput> Execute(int Id_RAC)
        {
            var rac = await _getRACReadOnlyRepository.GetRAC(Id_RAC);
            return new ImplementacionRACOutput(rac);
        }
    }
}
