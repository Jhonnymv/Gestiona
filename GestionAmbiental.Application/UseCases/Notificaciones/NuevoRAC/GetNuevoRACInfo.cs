using GestionAmbiental.Application.Repositories.RAC;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Notificaciones.NuevoRAC
{
    public class GetNuevoRACInfo : IGetNuevoRACInfo
    {
        private readonly IGetRACReadOnlyRepository _getRACReadOnlyRepository;
        public GetNuevoRACInfo(IGetRACReadOnlyRepository getRACReadOnlyRepository)
        {
            _getRACReadOnlyRepository = getRACReadOnlyRepository ?? throw new ArgumentNullException(nameof(getRACReadOnlyRepository));
        }

        public async Task<NuevoRACOutput> Execute(int IdRAC)
        {
            var rac = await _getRACReadOnlyRepository.GetRAC(IdRAC);
            return new NuevoRACOutput(rac);
        }
    }
}
