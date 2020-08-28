using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.AvistamientoFauna.ContactoEmergencia.AgregarContactoEmergencia
{
    public class AgregarContactoEmergencia : IAgregarContactoEmergencia
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public AgregarContactoEmergencia(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<int> Execute(string nombres, string apellidos, string telefono)
        {
            var input = new Domain.AvistamientoFauna.Items.ContactoEmergencia()
            {
                Nombres = nombres,
                Apellidos = apellidos,
                Telefono = telefono
            };
            return await _gestorTablasMaestrasReadOnlyRepository.AddContactoEmergencia(input);
        }
    }
}
