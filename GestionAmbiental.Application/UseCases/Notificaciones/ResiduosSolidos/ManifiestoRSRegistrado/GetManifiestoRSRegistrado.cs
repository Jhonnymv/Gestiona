using GestionAmbiental.Application.Repositories.ResiduosSolidos.ControlManifiestoRS;
using GestionAmbiental.Application.UseCases.ResiduosSolidos.ControlManifiestoRS;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Notificaciones.ResiduosSolidos.ManifiestoRSRegistrado
{
    public class GetManifiestoRSRegistrado : IGetManifiestoRSRegistrado
    {
        private readonly IGetManifiestosRSReadOnlyRepository _getManifiestosRSReadOnlyRepository;

        public GetManifiestoRSRegistrado(IGetManifiestosRSReadOnlyRepository getManifiestosRSReadOnlyRepository)
        {
            _getManifiestosRSReadOnlyRepository = getManifiestosRSReadOnlyRepository ?? throw new ArgumentNullException(nameof(getManifiestosRSReadOnlyRepository));
        }

        public async Task<ManifiestoRSOutput> Execute(int Id_Manifiesto)
        {
            var data = await _getManifiestosRSReadOnlyRepository.GetManifiestoRS(Id_Manifiesto);
            return new ManifiestoRSOutput(data);
        }
    }
}
