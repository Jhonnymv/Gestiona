using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.ACR_CMRS.GetACR_CMRS
{
    public class GetACR_CMRS : IGetACR_CMRS
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public GetACR_CMRS(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<ACR_CMRSOutput> Execute(int idACR_CMRS)
        {
            var acr_cmrs = await _gestorTablasMaestrasReadOnlyRepository.GetACR_CMRS(idACR_CMRS);
            var output = new ACR_CMRSOutput(acr_cmrs);
            return output;
        }
    }
}
