using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.ConductorRS.AgregarConductorRS
{
    public class AgregarConductorRS : IAgregarConductorRS
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public AgregarConductorRS(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<int> Execute(string Nombres, string Apellidos, string NLicencia)
        {
            var conductor = new Domain.ValueObjects.Conductor_RS
            {
                Nombres = Nombres,
                Apellidos = Apellidos,
                NLicencia = NLicencia
            };
            return await _gestorTablasMaestrasReadOnlyRepository.AddConductor_RS(conductor);
        }
    }
}
