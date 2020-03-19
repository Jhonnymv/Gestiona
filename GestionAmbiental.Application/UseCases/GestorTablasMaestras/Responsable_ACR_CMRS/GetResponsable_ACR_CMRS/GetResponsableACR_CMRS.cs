using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Responsable_ACR_CMRS.GetResponsable_ACR_CMRS
{
    public class GetResponsableACR_CMRS : IGetResponsableACR_CMRS
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public GetResponsableACR_CMRS(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<ResponsableACR_CMRSOutput> Execute(int idResponsable)
        {
            var responsable = await _gestorTablasMaestrasReadOnlyRepository.GetResponsable_ARC_CRMS(idResponsable);
            var output = new ResponsableACR_CMRSOutput(responsable);
            return output;
        }
    }
}
