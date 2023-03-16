using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestionAmbiental.Application.Repositories.CompromisosMCB.EtapaCMCB;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.CompromisosMCB.EtapaCMCB.DetalleRegistro
{
    public class DetalleRegistro : IDetalleRegistro
    {
        private readonly IEtapaReadOnlyRepository _etapaReadOnlyRepository;

        public DetalleRegistro(IEtapaReadOnlyRepository etapaReadOnlyRepository)
        {
            _etapaReadOnlyRepository = etapaReadOnlyRepository;
        }

        public async Task<EtapaOuput> Execute(int Id)
        {
            var etapa = await _etapaReadOnlyRepository.DetalleEtapa(Id);
            var output = new EtapaOuput(etapa);
            return output;
        }
    }
}