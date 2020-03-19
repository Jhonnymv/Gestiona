using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Responsable_ACR_CMRS.EditarResponsable_ACR_CMRS
{
    public class EditarResponsableACR_CMRS : IEditarResponsableARC_CMRS
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public EditarResponsableACR_CMRS(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task Execute(int IdResponsable, string Nombre, string Apellido, string Estado)
        {
            var responsable = new Domain.ValueObjects.Responsable_ARC_CRMS();
            responsable.Id_Responsable_ACR_CRMS = IdResponsable;
            responsable.Nombres = Nombre;
            responsable.Apellidos = Apellido;
            responsable.Estado = Estado;
            await _gestorTablasMaestrasReadOnlyRepository.UpdateResponsable_ARC_CRMS(responsable);
        }
    }
}
