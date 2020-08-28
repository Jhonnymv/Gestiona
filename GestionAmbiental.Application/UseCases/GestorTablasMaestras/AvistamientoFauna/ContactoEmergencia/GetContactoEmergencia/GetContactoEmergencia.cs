using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.AvistamientoFauna.ContactoEmergencia.GetContactoEmergencia
{
    public class GetContactoEmergencia : IGetContactoEmergencia
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public GetContactoEmergencia(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<ContactoEmergenciaOutput> Execute(int idContacto)
        {
            var contacto = await _gestorTablasMaestrasReadOnlyRepository.GetContactoEmergencia(idContacto);
            var output = new ContactoEmergenciaOutput(contacto);
            return output;
        }
    }
}
