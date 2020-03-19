using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.ConductorRS.EditarConductorRS
{
    public class EditarConductorRS : IEditarConductorRS
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public EditarConductorRS(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task Execute(int IdConductor, string Nombres, string Apellidos, string NLicencia)
        {
            var conductor = new Domain.ValueObjects.Conductor_RS();
            conductor.Id_Conductor_RS = IdConductor;
            conductor.Nombres = Nombres;
            conductor.Apellidos = Apellidos;
            conductor.NLicencia = NLicencia;
            await _gestorTablasMaestrasReadOnlyRepository.UpdateConductor_RS(conductor);
        }
    }
}
