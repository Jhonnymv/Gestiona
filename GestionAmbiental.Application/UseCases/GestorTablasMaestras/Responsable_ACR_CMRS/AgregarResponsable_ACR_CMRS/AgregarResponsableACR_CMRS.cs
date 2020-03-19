using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Responsable_ACR_CMRS.AgregarResponsable_ACR_CMRS
{
    public class AgregarResponsableACR_CMRS : IAgregarResponsableACR_CMRS
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public AgregarResponsableACR_CMRS(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<int> Execute(string Nombre, string Apellido)
        {
            var responsable = new Domain.ValueObjects.Responsable_ARC_CRMS
            {
                Nombres = Nombre,
                Apellidos = Apellido
            };
            return await _gestorTablasMaestrasReadOnlyRepository.AddResponsable_ARC_CRMS(responsable);
        }
    }
}
