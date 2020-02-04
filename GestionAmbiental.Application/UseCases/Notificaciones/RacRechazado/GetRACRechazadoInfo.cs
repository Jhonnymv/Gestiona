using GestionAmbiental.Application.Repositories.Notificaciones;
using GestionAmbiental.Application.Repositories.RAC;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Notificaciones.RacRechazado
{
    public class GetRACRechazadoInfo : IGetRACRechazadoInfo
    {
        private readonly IGetRACReadOnlyRepository _getRACReadOnlyRepository;

        public GetRACRechazadoInfo(IGetRACReadOnlyRepository getRACReadOnlyRepository)
        {
            _getRACReadOnlyRepository = getRACReadOnlyRepository ?? throw new ArgumentNullException(nameof(getRACReadOnlyRepository));
        }

        public async Task<RacRechazadoOutput> Execute(int Id_RAC)
        {
            var rac = await _getRACReadOnlyRepository.GetRAC(Id_RAC);
            return new RacRechazadoOutput(rac);
        }
    }
}
