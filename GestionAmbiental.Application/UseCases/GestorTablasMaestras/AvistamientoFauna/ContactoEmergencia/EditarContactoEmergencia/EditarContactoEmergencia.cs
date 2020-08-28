using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.AvistamientoFauna.ContactoEmergencia.EditarContactoEmergencia
{
    public class EditarContactoEmergencia : IEditarContactoEmergencia
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public EditarContactoEmergencia(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task Execute(int id, string nombres, string apellidos, string telefono, string estado)
        {
            var input = new Domain.AvistamientoFauna.Items.ContactoEmergencia();
            input.IdContactoEmergencia = id;
            input.Nombres = nombres;
            input.Apellidos = apellidos;
            input.Telefono = telefono;
            input.Estado = estado;
            await _gestorTablasMaestrasReadOnlyRepository.UpdateContactoEmergencia(input);
        }
    }
}
