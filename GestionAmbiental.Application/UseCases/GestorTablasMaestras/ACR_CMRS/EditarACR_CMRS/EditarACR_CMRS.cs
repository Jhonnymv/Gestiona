using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.ACR_CMRS.EditarACR_CMRS
{
    public class EditarACR_CMRS : IEditarACR_CMRS
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public EditarACR_CMRS(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task Execute(int idACR_CMRS, string descripcion,string estado)
        {
            var acr_cmrs = new Domain.ValueObjects.ACR_CMRS();
            acr_cmrs.Id_ACR_CMRS = idACR_CMRS;
            acr_cmrs.Descripcion = descripcion;
            acr_cmrs.Estado = estado;
            await _gestorTablasMaestrasReadOnlyRepository.UpdateACR_CMRS(acr_cmrs);
        }
    }
}
